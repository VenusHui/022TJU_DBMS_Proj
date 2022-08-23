using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudyPlat.Entities;
using StudyPlat.Message;
using StudyPlat.Models;


namespace StudyPlat.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class QueryController : ControllerBase
    {
        private readonly ModelContext _context;
        private static object obj = new object();
        public QueryController(ModelContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 获取特定题目，根据question_id来取回question可能会用到的数据，参数：question_id
        /// </summary>
        /// <remarks>
        /// answer_id_list是一个最多包含5个答案ID的数组
        /// pic_url是图片的相应地址，可能为空，此时答案不附图片
        /// 返回题目信息，返回信息示例 :
        ///     
        ///     Get/sample
        ///     {
        ///         "header":
        ///         {
        ///             "code" : 0,
        ///             "message" : "获取题目信息成功"
        ///         },
        ///         "data":
        ///         {
        ///             "pic_url" : "http//:",
        ///             "answer_id_list": [""]
        ///             "question_stem":"题干信息",
        ///             "question_id":"1",
        ///             "post_time" :""
        ///         }
        ///     }
        ///  code对应的情况:
        ///  0:获取题目信息成功
        ///  -1:没有相应题目信息，请检查ID是否出错
        /// </remarks>
        /// <param name="question_id"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseCache(Duration = 10, VaryByQueryKeys = new string[] { "question_id" })]
        public IActionResult GetQuestion(string question_id)
        {
            lock(obj)
            {
                MQuestion mQuestion = new MQuestion(_context);
                MAnswer mAnswer = new MAnswer(_context);
                Question question = mQuestion.GetQuestion(question_id);
                //如果出现-1的id说明出现了错误
                if (question.QuestionId == "-1")
                {
                    return new JsonResult(new QuestionMessage
                    {
                        header = new Header
                        {
                            code = -1,
                            message = "没有相应的题目信息,请检查ID是否出错"
                        },
                        data = new QuestionData
                        {
                            pic_url = "",
                            answer_id_list = new string[50]
                        }
                    });
                }
                string Qid = question.QuestionId;
                string[] answerIdArray = new string[5];
                answerIdArray = mAnswer.GetAnswerIdArray(Qid);

                return new JsonResult(new QuestionMessage
                {
                    header = new Header
                    {
                        code = 0,
                        message = "获取题目信息成功"
                    },
                    data = new QuestionData
                    {
                        question_stem = question.QuestionStem,
                        pic_url = question.PicUrl,
                        answer_id_list = answerIdArray,
                        question_id = question.QuestionId,
                        post_time = question.PostTime  
                    }
                });
            }
        }

        /// <summary>
        /// 根据answer_id来获取特定答案，参数:answer_id
        /// </summary>
        /// <remarks>
        /// answer_id_list是一个最多包含5个答案ID的数组
        /// pic_url是图片的相应地址，可能为空，此时答案不附图片
        /// 返回答案信息，返回信息示例 :
        ///     
        ///     Get/sample
        ///     {
        ///         "header":
        ///         {
        ///             "code" : 0,
        ///             "message" : "获取答案信息成功"
        ///         },
        ///         "data":
        ///         {
        ///             "answer_content" : "这道题目的解答思路是",
        ///             "answer_id": "001"
        ///         }
        ///     }
        ///  code对应的情况:
        ///  0:根据答案id获取答案信息成功
        ///  -1:没有相应的答案信息，请检查ID是否出错
        /// </remarks>
        /// <param name="answer_id"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseCache(Duration = 10, VaryByQueryKeys = new string[] { "answer_id" })]
        public IActionResult GetAnswer([FromQuery] string answer_id)
        {
            lock(obj)
            {
                MAnswer mAnswer = new MAnswer(_context);
                Answer answer = mAnswer.GetAnswer(answer_id);
                //查找失败
                if (answer.AnswerId == "-1")
                {
                    return new JsonResult(new AnswerMessage
                    {
                        header = new Header
                        {
                            code = -1,
                            message = "没有相应的答案信息，请检查ID是否出错"
                        },
                        data = new AnswerData
                        {
                            answer_content = "-1",
                            answer_id = "-1"
                        }
                    });
                }
                else
                {
                    return new JsonResult(new AnswerMessage
                    {
                        header = new Header
                        {
                            code = 0,
                            message = "根据答案id获取答案信息成功"
                        },
                        data = new AnswerData
                        {
                            answer_content = answer.AnswerContent,
                            answer_id = answer.AnswerId
                        }
                    });
                }
            }
        }

        /// <summary>
        /// 根据isbn取一个特定的书本信息，参数:isbn
        /// </summary>
        /// <remarks>
        /// 返回书籍信息，返回信息示例 :
        ///     
        ///     Get/sample
        ///     {
        ///         "header":
        ///         {
        ///             "code" : 0,
        ///             "message" : "根据书本id获取书本信息成功"
        ///         },
        ///         "data":
        ///         {
        ///             "isbn" : "12346",
        ///             "book_name": "高等数学",
        ///             "author" :"作者",
        ///             "publish_time" : "一个时间戳",
        ///             "publisher" : "出版社",
        ///             "comprehension" : "简介",
        ///             "pic_url": "https//:"
        ///         }
        ///     }
        ///  code对应的情况:
        ///  0:根据书本id获取书本信息成功
        ///  -1:没有相应的书本信息，请检查Isbn是否出错
        /// </remarks>
        /// <param name="isbn"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseCache(Duration = 10, VaryByQueryKeys = new string[] { "isbn" })]
        public IActionResult GetBook([FromQuery] string isbn)
        {
            lock(obj)
            {
                MBook mBook = new MBook(_context);
                Book book = mBook.GetBook(isbn);
                if (book.Isbn == "-1")
                {
                    return new JsonResult(new BookMessage
                    {
                        header = new Header
                        {
                            code = -1,
                            message = "没有相应的书本信息，请检查Isbn是否出错"
                        },
                        data = new BookData
                        {
                            isbn = "-1",
                            book_name = "-1",
                            author = "-1",
                            publish_time = DateTime.Now,
                            publisher = "-1",
                            comprehension = "-1",
                            pic_url = "-1"
                        }
                    });
                }
                else
                {
                    return new JsonResult(new BookMessage
                    {
                        header = new Header
                        {
                            code = 0,
                            message = "根据书本id获取书本信息成功"
                        },
                        data = new BookData
                        {
                            isbn = book.Isbn,
                            book_name = book.BookName,
                            author = book.Author,
                            publish_time = book.PublishTime,
                            publisher = book.Publisher,
                            comprehension = book.Comprehension,
                            pic_url = book.PicUrl
                        }
                    });
                }
            }
        }

        /// <summary>
        /// 根据course_id来取一个特定的课程，参数:course_id
        /// </summary>
        /// <remarks>
        /// 返回课程信息，返回信息示例 :
        ///     
        ///     Get/sample
        ///     {
        ///         "header":
        ///         {
        ///             "code" : 0,
        ///             "message" : "根据课程id获取课程信息成功"
        ///         },
        ///         "data":
        ///         {
        ///             "course_id" : "12346",
        ///             "comprehension": "有关计算机硬件的一门课",
        ///             "course_name" :"计算机系统结构",
        ///             "pic_url" : "https://"
        ///         }
        ///     }
        ///  code对应的情况:
        ///  0:根据课程id获取课程信息成功
        ///  -1:没有相应的课程信息，请检查课程ID是否出错
        ///  </remarks>
        /// <param name="course_id"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseCache(Duration =10 , VaryByQueryKeys =new string[] { "course_id"})]
        public IActionResult GetCourse([FromQuery] string course_id)
        {
            lock(obj)
            {
                MCourse mCourse = new MCourse(_context);
                Course course = mCourse.GetCourse(course_id);
                if (course.CourseId == "-1")
                {
                    return new JsonResult(new CourseMessage
                    {
                        header = new Header
                        {
                            code = -1,
                            message = "没有相应的课程信息，请检查课程ID是否出错"
                        },
                        data = new CourseData
                        {
                            course_id = "-1",
                            comprehension = "-1",
                            course_name = "-1"
                        }
                    });
                }
                else
                {
                    return new JsonResult(new CourseMessage
                    {
                        header = new Header
                        {
                            code = 0,
                            message = "根据课程id获取课程信息成功"
                        },
                        data = new CourseData
                        {
                            course_id = course_id,
                            comprehension = course.Comprehension,
                            course_name = course.CourseName,
                            pic_url = course.PicUrl
                        }
                    });
                }
            }
        }

        /// <summary>
        /// 通过关键字搜索题目，参数:text
        /// </summary>
        /// <remarks>
        /// IdList是一个装有相关题目ID的链表
        /// 返回搜索题目信息，返回信息示例 :
        /// 
        ///     Get/sample
        ///     {
        ///         "header":
        ///         {
        ///             "code" : 0,
        ///             "message" : "获取相关题目ID成功"
        ///         },
        ///         "data":
        ///         {
        ///             "IdList" : [""],
        ///         }
        ///     }
        ///  code对应的情况:
        ///  0:获取相关题目ID成功
        /// </remarks>
        /// <param name="text"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseCache(Duration = 10, VaryByQueryKeys = new string[] {"text" })]
        public IActionResult QueryQuestion([FromQuery] string text)
        {
            //查找的结果为所有可能对应的题目的id
            lock(obj)
            {
                MQuestion mQuestion = new MQuestion(_context);
                List<string> questionList = mQuestion.QueryQuestion(text);

                return new JsonResult(new QueryMessage
                {
                    header = new Header
                    {
                        code = 0,
                        message = "获取相关题目ID成功"
                    },
                    data = new QueryData
                    {
                        IdList = questionList
                    }
                });
            }
        }


        /// <summary>
        /// 通过关键字搜索书籍，参数；text
        /// </summary>
        /// <remarks>
        /// IdList是一个装有相关书籍isbn的链表
        /// 返回搜索书籍信息，返回信息示例 :
        /// 
        ///     Get/sample
        ///     {
        ///         "header":
        ///         {
        ///             "code" : 0,
        ///             "message" : "获取相关书籍isbn码成功"
        ///         },
        ///         "data":
        ///         {
        ///             "IdList" : [""],
        ///         }
        ///     }
        ///  code对应的情况:
        ///  0:获取相关书籍isbn码成功
        /// </remarks>
        /// <param name="text"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseCache(Duration = 10, VaryByQueryKeys =new string[]{"text"})]
        public IActionResult QueryBook([FromQuery] string text)
        {
            lock(obj)
            {
                MBook mBook = new MBook(_context);
                List<string> bookList = mBook.QueryBook(text);
                return new JsonResult(new QueryMessage
                {
                    header = new Header
                    {
                        code = 0,
                        message = "获取相关书籍isbn码成功"
                    },
                    data = new QueryData
                    {
                        IdList = bookList
                    }
                });
            }
        }

        /// <summary>
        /// 通过关键字搜索相关课程，参数:text
        /// </summary>
        /// <remarks>
        /// IdList是一个装有相关课程id的链表
        /// 返回搜索课程信息，返回信息示例 :
        /// 
        ///     Get/sample
        ///     {
        ///         "header":
        ///         {
        ///             "code" : 0,
        ///             "message" : "获取相关课程ID成功"
        ///         },
        ///         "data":
        ///         {
        ///             "IdList" : [""],
        ///         }
        ///     }
        ///  code对应的情况:
        ///  0:获取相关课程ID成功
        /// </remarks>
        /// <param name="text"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseCache(Duration = 10 , VaryByQueryKeys = new string[] { "text"})]
        public IActionResult QueryCourse([FromQuery] string text)
        {
            lock(obj)
            {
                MCourse mCourse = new MCourse(_context);
                List<string> IdList = mCourse.QueryCourse(text);
                return new JsonResult(new QueryMessage
                {
                    header = new Header
                    {
                        code = 0,
                        message = "获取相关课程ID成功"
                    },
                    data = new QueryData
                    {
                        IdList = IdList
                    }
                });
            }
        }

        /// <summary>
        /// 用于在收藏夹界面搜索题目用的接口，参数:user_id/text
        /// 返回符合条件的题目的IDList
        /// </summary>
        /// <remarks>
        /// 返回信息实例:
        /// 
        ///     Get/Sample:
        ///     {
        ///         "header":
        ///         {
        ///             "code" = 0,
        ///             "message" = "搜索到了该用户收藏题目ID的List数据"
        ///         },
        ///         "data":
        ///         {
        ///             IdList = ["1"]
        ///         }
        ///     }
        ///     
        /// code对应的情况:
        /// 0:搜索到了该用户收藏题目ID的List数据
        /// -1:参数user_id或text为空，请检查
        /// </remarks>
        /// <param name="user_id"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult QueryCollectionQuestion([FromQuery]string user_id,[FromQuery]string text)
        {
            lock(obj)
            {
                MQuestion mQuestion = new MQuestion(_context);
                List<string> QueryQuestionIDList = mQuestion.QueryCollectionQuestion(user_id, text);
                if (user_id == null || text == null)
                    return new JsonResult(new QueryMessage
                    {
                        header = new Header
                        {
                            code = -1,
                            message = "参数user_id或text为空，请检查"
                        },
                        data = new QueryData
                        {
                            IdList = QueryQuestionIDList
                        }
                    });
                return new JsonResult(new QueryMessage
                {
                    header = new Header
                    {
                        code = 0,
                        message = "搜索到了该用户收藏题目ID的List数据"
                    },
                    data = new QueryData
                    {
                        IdList = QueryQuestionIDList
                    }
                });
            }
        }

        /// <summary>
        /// 用于在收藏夹界面搜索书本用的接口，参数:user_id/text
        /// 返回符合条件的书本的ISBN的list
        /// </summary>
        /// <remarks>
        /// 返回信息实例:
        /// 
        ///     Get/Sample:
        ///     {
        ///         "header":
        ///         {
        ///             "code" = 0,
        ///             "message" = "搜索到了该用户收藏书本的ISBN码的List数据"
        ///         },
        ///         "data":
        ///         {
        ///             IdList = ["1"]
        ///         }
        ///     }
        ///     
        /// code对应的情况:
        /// 0:搜索到了该用户收藏书本的ISBN码的List数据
        /// -1:参数user_id或text为空,请检查
        /// </remarks>>
        /// <param name="user_id"></param>
        /// <param name="text"></param>
        /// <returns></returns>

        [HttpGet]
        public IActionResult QueryCollectionBook(string user_id,string text)
        {
            lock(obj)
            {
                MBook mBook = new MBook(_context);
                List<string> QueryBookIDList = mBook.QueryBookCollection(user_id, text);

                if (user_id == null || text == null)
                    return new JsonResult(new QueryMessage
                    {
                        header = new Header
                        {
                            code = -1,
                            message = "参数user_id或text为空，请检查"
                        },
                        data = new QueryData
                        {
                            IdList = QueryBookIDList
                        }
                    });

                return new JsonResult(new QueryMessage
                {
                    header = new Header
                    {
                        code = 0,
                        message = "搜索到了该用户收藏书本的ISBN码的List数据"
                    },
                    data = new QueryData
                    {
                        IdList = QueryBookIDList
                    }
                });
            }
        }
        /// <summary>
        /// 用于在收藏夹界面搜索课程用到的接口，参数:user_id/text
        /// 返回符合条件的课程的IDList
        /// </summary>
        /// <remarks>
        /// 返回信息实例:
        /// 
        ///     Get/Sample:
        ///     {
        ///         "header":
        ///         {
        ///             "code" = 0,
        ///             "message" = "搜索到了该用户收藏书本的ID的List数据"
        ///         },
        ///         "data":
        ///         {
        ///             IdList = ["1"]
        ///         }
        ///     }
        ///     
        /// code对应的情况:
        /// 0:搜索到了该用户收藏书本的ID的List数据
        /// -1:参数user_id或text为空,请检查
        /// </remarks>
        /// <param name="user_id"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult QueryCollectionCourse(string user_id, string text)
        {
            lock (obj)
            {
                MCourse mCourse = new MCourse(_context);
                List<string> QueryCourseIDList = mCourse.QueryCourseCollection(user_id, text);
                if (user_id == null || text == null)
                    return new JsonResult(new QueryMessage
                    {
                        header = new Header
                        {
                            code = -1,
                            message = "参数user_id或text为空，请检查"
                        },
                        data = new QueryData
                        {
                            IdList = QueryCourseIDList
                        }
                    });
                return new JsonResult(new QueryMessage
                {
                    header = new Header
                    {
                        code = 0,
                        message = "搜索到了该用户收藏书本的ID的List数据"
                    },
                    data = new QueryData
                    {
                        IdList = QueryCourseIDList
                    }
                });
            }
        }

        /// <summary>
        /// 这是用于获得特定用户当前已收藏所有题目id的api,
        /// </summary>
        /// <remarks>
        /// idArray是一个大小为50的string数组，包含有所有收藏题目的ID信息 :
        ///     
        ///     Get/sample
        ///     {
        ///         "header":
        ///         {
        ///             "code" : 0,
        ///             "message" : "返回了所有收藏题目的ID信息"
        ///         },
        ///         "data":
        ///         {
        ///             "idArray" : [""]
        ///         }
        ///     }
        /// </remarks>
        /// <param name="user_id"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseCache(Duration = 10, VaryByQueryKeys = new string[] { "user_id" })]
        public IActionResult GetCollectionQuestion([FromQuery] string user_id)
        {
            lock (obj)
            {
                MQuestion mQuestion = new MQuestion(_context);
                string[] questionIdArray = new string[50];
                questionIdArray = mQuestion.GetQuestionCollection(user_id);
                return new JsonResult(new CollectionMessage
                {
                    header = new Header
                    {
                        code = 0,
                        message = "返回了所有收藏题目的ID信息"
                    },
                    data = new CollectionData
                    {
                        idArray = questionIdArray
                    }
                });
            }
        }
        /// <summary>
        /// 用于获得收藏书本的所有ID
        /// </summary>
        /// <remarks>
        /// idArray是一个大小为50的string数组，包含有所有收藏书本的信息 :
        ///     
        ///     Get/sample
        ///     {
        ///         "header":
        ///         {
        ///             "code" : 0,
        ///             "message" : "返回了所有收藏书本的isbn码信息"
        ///         },
        ///         "data":
        ///         {
        ///             "idArray" : [""]
        ///         }
        ///     }
        /// </remarks>
        /// <param name="user_id"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseCache(Duration = 10, VaryByQueryKeys = new string[] { "user_id" })]
        public IActionResult GetCollectionBook([FromQuery] string user_id)
        {
            lock(obj)
            {
                MBook mBook = new MBook(_context);
                string[] bookIdCollection = new string[50];
                bookIdCollection = mBook.GetBookCollection(user_id);
                return new JsonResult(new CollectionMessage
                {
                    header = new Header
                    {
                        code = 0,
                        message = "返回了所有收藏书本的isbn码信息"
                    },
                    data = new CollectionData
                    {
                        idArray = bookIdCollection
                    }
                });
            }
        }
        /// <summary>
        /// 用于获得收藏课程的所有ID
        /// </summary>
        /// <remarks>
        /// idArray是一个大小为50的string数组，包含有所有课程ID的信息 :
        ///     
        ///     Get/sample
        ///     {
        ///         "header":
        ///         {
        ///             "code" : 0,
        ///             "message" : "返回了所有收藏课程的ID信息"
        ///         },
        ///         "data":
        ///         {
        ///             "idArray" : [""]
        ///         }
        ///     }
        /// </remarks>
        /// <param name="user_id"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseCache(Duration = 10, VaryByQueryKeys = new string[] { "user_id" })]
        public IActionResult GetCollectionCourse([FromQuery] string user_id)
        {lock (obj)
            {
                MCourse mCourse = new MCourse(_context);
                string[] courseIdCollection = new string[50];
                courseIdCollection = mCourse.GetCourseCollection(user_id);
                return new JsonResult(new CollectionMessage { header = new Header { code = 0, message = "返回了所有收藏课程的ID信息" }, data = new CollectionData { idArray = courseIdCollection } });
            }
        }
        /// <summary>
        /// 获得专家答题界面所需展现的问题的IDList，参数：expert_id
        /// </summary>
        /// <remarks>
        /// finishedIDList:该专家已回答过的问题的idlist
        /// unfinishedIDList:待该专家回答的问题的idList
        /// 返回信息实例:
        ///     Get/Sample:
        ///     {
        ///         "header":
        ///         {
        ///             "code" : 0,
        ///             "message" : "获得该专家回答过的题目的IDList和待回答题目IDList"
        ///         },
        ///         "data":
        ///         {
        ///             "finishedIDList" : ["1"],
        ///             "unfinishedIDList" : ["2","3"]
        ///         }
        ///     }
        /// </remarks>>
        /// <param name="expert_id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetQuestionForExpert(string expert_id)
        {
            lock(obj)
            {
                MUser mUser = new MUser(_context);
                MMajor mMajor = new MMajor(_context);
                string major_id = mMajor.GetMajorIDFromExpertID(expert_id);
                List<string> finishedList = mUser.FindFinishedQuestion(expert_id);
                List<string> unfinishedList = mUser.FindUnfinishedQuestion(major_id);
                return new JsonResult(new ExpertMessage
                {
                    data = new ExpertData
                    {
                        finishedIDList = finishedList,
                        unfinishedIDList = unfinishedList
                    },
                    header = new Header
                    {
                        code = 0,
                        message = "获得该专家回答过的题目的IDList和待回答题目IDList"
                    }
                });
            }
        }
    }
}

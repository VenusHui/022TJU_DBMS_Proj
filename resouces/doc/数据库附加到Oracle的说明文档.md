# 数据库设计

## 1.1 表的设计

1. 用户表：在线学习平台的使用者，记录用户ID，用户名，用户手机，学校名，密码和用户专业以及用户类型。
2. 书籍表：学习平台所收录课本，记录书本ISBN、书籍名、书籍作者、书籍出版社、书籍出版时间、书籍简介以及书籍封面图片URL。
3. 课程表：学习平台所收录课程，记录课程ID和课程名称以及课程简介以及课程图片URL。
4. 题目表：学习平台所收录题目，记录题目ID、题目题干、题目状态、题目图片URL和题目发布时间戳。
5. 答案表：题目所对应的答案，记录回答ID、答案内容和答案赞同数。
6. 专业表：学习平台所收录专业，记录专业ID以及专业名称。
7. 反馈信息表：用户向平台管理员所反馈的信息，记录反馈信息ID、反馈的内容、反馈发布时间戳和反馈回复。
8. 课程-书籍表：根据联系集转换而来，记录课程ID、书籍编码和添加该书的日期。
9. 专业-课程表：根据联系集转换而来，记录专业ID、课程ID。
10. 专家-课程表：根据联系集转换而来，记录专业ID、课程ID。
11. 问题-专业表：根据联系集转换而来，记录题目ID、专业ID。
12. 专家回答表：根据联系集转换而来，记录专家ID、答案ID、添加答案的日期。
13. 书中题目表：记录出自书中的题目信息，记录题目ID，书的ISBN以及题目所在页码。
14. 课程中题目表：记录出自课程中的题目信息，记录题目ID，课程ID。
15. 题目收藏表：记录用户收藏的题目，记录题目ID，用户ID，用户的笔记以及收藏时间。
16. 课程收藏夹：记录用户收藏的课程，记录课程ID，用户ID，以及收藏时间。
17. 书籍收藏表：记录用户收藏的书籍，记录课程ID，用户ID，以及收藏时间。
18. 题目解答表：记录题目的答案，记录题目ID，答案ID和回答问题的时间。
19. 用户赞同表：记录用户的点赞记录，记录。
    

### 1.1.1 用户表(user)

| 字段名       | 数据类型 | 长度 | 说明     | 备注     |
| ------------ | -------- | ---- | -------- | -------- |
| user_id      | VARCHAR  | 50   | 用户ID   | PK       |
| user_name    | VARCHAR  | 255  | 用户名   |          |
| password     | VARCHAR  | 50   | 用户密码 | 不为Null |
| user_type    | NUMBER   | 4    | 用户类型 | 不为Null |
| phone_number | VARCHAR  | 50   | 用户手机 | 不为Null |
| school_name  | VARCHAR  | 255  | 学校名   |          |
| major_id     | VARCHAR  | 50   | 用户专业 |          |

### 1.1.2 书籍表(book)

| 字段名        | 数据类型  | 长度 | 说明            | 备注               |
| ------------- | --------- | ---- | --------------- | ------------------ |
| isbn          | VARCHAR   | 50   | 书本isbn        | PK，书籍唯一识别码 |
| book_name     | VARCHAR   | 255  | 书籍名          | 不为Null           |
| author        | VARCHAR   | 255  | 书籍作者        |                    |
| publisher     | VARCHAR   | 255  | 书籍出版社      | 不为Null           |
| publish_time  | TIMESTAMP | N/A  | 书籍出版时间    | 不为Null           |
| comprehension | CLOB      | 4000 | 书籍简介        |                    |
| pic_url       | VARCHAR   | 255  | 书籍封面图片URL | 不为Null           |

### 1.1.3 课程表(course)

| 字段名        | 数据类型 | 长度 | 说明        | 备注     |
| ------------- | -------- | ---- | ----------- | -------- |
| course_id     | VARCHAR  | 50   | 课程ID      | PK       |
| course_name   | VARCHAR  | 255  | 课程名称    | 不为Null |
| comprehension | CLOB     | 4000 | 课程简介    |          |
| pic_url       | VARCHAR  | 255  | 课程图片URL | 不为Null |

### 1.1.4 题目表(question)

| 字段名        | 数据类型  | 长度 | 说明             | 备注                           |
| ------------- | --------- | ---- | ---------------- | ------------------------------ |
| question_id   | VARCHAR   | 50   | 题目ID           | PK                             |
| question_stem | CLOB      | 4000 | 题目题干         | 描述题目                       |
| status        | NUMBER    | 1    | 题目状态         | 0为未回答，1为已回答，不为Null |
| pic_url       | VARCHAR   | 255  | 题目图片         |                                |
| post_time     | TIMESTAMP | N/A  | 题目发布时间  戳 | 不为Null                       |

### 1.1.5 答案表(answer)

| 字段名          | 数据类型 | 长度 | 说明       | 备注     |
| --------------- | -------- | ---- | ---------- | -------- |
| answer _id      | VARCHAR  | 50   | 答案ID     | PK       |
| answer _content | CLOB     | 4000 | 答案内容   | 不为Null |
| approve         | NUMBER   | 35   | 答案赞同数 | 不为Null |

### 1.1.6 专业表(major)

| 字段名      | 数据类型 | 长度 | 说明     | 备注     |
| ----------- | -------- | ---- | -------- | -------- |
| major _id   | VARCHAR  | 50   | 专业ID   | PK       |
| major _name | VARCHAR  | 255  | 专业名称 | 不为Null |

### 1.1.7 反馈信息表(feedback_info)

| 字段名      | 数据类型  | 长度 | 说明             | 备注                                   |
| ----------- | --------- | ---- | ---------------- | -------------------------------------- |
| feedback_id | VARCHAR   | 50   | 反馈信息ID       | PK                                     |
| content     | VARCHAR   | 1000 | 反馈的内容       | 反馈用户填写                           |
| post_time   | TIMESTAMP | N/A  | 反馈发布时间  戳 | 用户发布反馈信息的时间，不为Null       |
| reply       | VARCHAR   | 1000 | 反馈回复         | 处理反馈的管理员填写，  未处理时可为空 |
| is_finished | NUMBER    | 1    | 反馈是否处理     | 0未处理，1已处理，不为Null             |

### 1.1.8 课程-书籍表(has_book)

| 字段名        | 数据类型 | 长度 | 说明     | 备注                           |
| ------------- | -------- | ---- | -------- | ------------------------------ |
| course_id     | VARCHAR  | 50   | 课程ID   | PK:FK，参照课程表的  course_id |
| isbn          | VARCHAR  | 50   | 书籍编码 | PK:FK，参照书籍表的isbn        |
| addition_date | DATE     | 7    | 添加日期 | 不为Null                       |

### 1.1.9 专业-课程表(course_from_major)

| 字段名    | 数据类型 | 长度 | 说明   | 备注                           |
| --------- | -------- | ---- | ------ | ------------------------------ |
| major _id | VARCHAR  | 50   | 专业ID | PK                             |
| course_id | VARCHAR  | 50   | 课程ID | PK:FK，参照课程表的  course_id |

### 1.1.10 专家-专业表(has_expert)

| 字段名    | 数据类型 | 长度 | 说明   | 备注                           |
| --------- | -------- | ---- | ------ | ------------------------------ |
| expert_id | VARCHAR  | 50   | 专家ID | PK:FK，参照专家表的  expert_id |
| major_id  | VARCHAR  | 50   | 专业ID | PK:FK，参照专业表的  major_id  |

### 1.1.11 问题-专业表(question_from_major)

| 字段名      | 数据类型 | 长度 | 说明   | 备注                             |
| ----------- | -------- | ---- | ------ | -------------------------------- |
| question_id | VARCHAR  | 50   | 问题ID | PK:FK，参照问题表的  question_id |
| major_id    | VARCHAR  | 50   | 专业ID | PK:FK，参照专业表的  major_id    |

### 1.1.12 专家回答表(give_answer)

| 字段名        | 数据类型 | 长度 | 说明     | 备注                           |
| ------------- | -------- | ---- | -------- | ------------------------------ |
| expert_id     | VARCHAR  | 50   | 专家ID   | PK:FK，参照专家表的  expert_id |
| answer_id     | VARCHAR  | 50   | 答案ID   | PK:FK，参照答案表的  answer_id |
| addition_date | DATE     | 7    | 添加日期 | 不为Null                       |

### 1.1.13 书中题目表(question_from_book)

| 字段名      | 数据类型 | 长度 | 说明             | 备注                                |
| ----------- | -------- | ---- | ---------------- | ----------------------------------- |
| question_id | VARCHAR  | 50   | 题目编号         | PK，FK，参照题目表中  的question_id |
| isbn        | VARCHAR  | 50   | 书的ISBN         | FK,参照书信息表中的isbn             |
| page        | NUMBER   | 5    | 题目所在页的页码 |                                     |

### 1.1.14 课程中题目表(question_from_course)

| 字段名      | 数据类型 | 长度 | 说明     | 备注                                 |
| ----------- | -------- | ---- | -------- | ------------------------------------ |
| question_id | VARCHAR  | 50   | 问题ID   | PK:FK，参照问题表的  question_id     |
| course_id   | VARCHAR  | 50   | 课程编号 | PK，FK,参照课程信息表中的  course_id |

### 1.1.15 题目收藏表(question_collection)

| 字段名      | 数据类型 | 长度 | 说明     | 备注                                   |
| ----------- | -------- | ---- | -------- | -------------------------------------- |
| question_id | VARCHAR  | 50   | 题目编号 | PK:FK，参照题目信息表  中的question_id |
| user_id     | VARCHAR  | 50   | 用户编号 | PK:FK,参照用户信息表中  的user_id      |
| note        | VARCHAR  | 1000 | 笔记     |                                        |

### 1.1.16 课程收藏表(course_collection)

| 字段名       | 数据类型  | 长度 | 说明     | 备注                                   |
| ------------ | --------- | ---- | -------- | -------------------------------------- |
| course_id    | VARCHAR   | 50   | 题目编号 | PK:FK，参照题目信息表  中的question_id |
| user_id      | VARCHAR   | 50   | 用户编号 | PK:FK,参照用户信息表中  的user_id      |
| collect_time | TIMESTAMP | N/A  | 收藏时间 | 不为Null                               |

### 1.1.17 书籍收藏表(book_collection)

| 字段名       | 数据类型  | 长度 | 说明     | 备注                                   |
| ------------ | --------- | ---- | -------- | -------------------------------------- |
| book_id      | VARCHAR   | 50   | 题目编号 | PK:FK，参照题目信息表  中的question_id |
| user_id      | VARCHAR   | 50   | 用户编号 | PK:FK,参照用户信息表中  的user_id      |
| collect_time | TIMESTAMP | N/A  | 收藏时间 | 不为Null                               |

### 1.1.18 题目解答表(explain_question)

| 字段名       | 数据类型  | 长度 | 说明     | 备注                                   |
| ------------ | --------- | ---- | -------- | -------------------------------------- |
| question_id  | VARCHAR   | 50   | 题目编号 | PK:FK，参照题目信息表  中的question_id |
| user_id      | VARCHAR   | 50   | 用户编号 | PK:FK,参照用户信息表中  的user_id      |
| collect_time | TIMESTAMP | N/A  | 收藏时间 | 不为Null                               |

### 1.1.19 用户赞同表(user_approve_answer)

| 字段名    | 数据类型 | 长度 | 说明     | 备注                                |
| --------- | -------- | ---- | -------- | ----------------------------------- |
| user_id   | VARCHAR  | 50   | 用户编号 | PK:FK,参照用户信息表中  的user_id   |
| answer_id | VARCHAR  | 50   | 课程编号 | PK:FK,参照答案信息表  中的answer_id |

## 1.2 数据库关系库

![pic](..\diagrams\数据库关系图.svg)
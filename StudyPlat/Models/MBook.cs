using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudyPlat.Entities;


namespace StudyPlat.Models
{
    public class MBook : Book
    {
        public readonly ModelContext _context;

        public MBook(ModelContext context)
        {
            CollectionBook = new HashSet<CollectionBook>();
            HasBook = new HashSet<HasBook>();
            QuestionFromBook = new HashSet<QuestionFromBook>();
            _context = context;
        }

        public int AddBook(Book book)
        {
            string isbn = book.Isbn;
            int num = this.CheckBook(isbn);
            if(num >= 1 )
            {
                return -1;//说明已经有这本书了
            }
            else
            {
                _context.Add(book);
                _context.SaveChanges();
                return 0;//说明成功保存
            }
        }

        public Book GetBook(string isbn)
        {
            IQueryable<Book> books = _context.Book;
            books = books.Where(u => u.Isbn == isbn);
            Book book;
            int num = books.Count();
            if(num == 1)
            {
                book = books.First();
            }
            else
            {
                book = new Book
                {
                    Isbn = "-1",
                    PicUrl = "-1"
                };
            }
            return book;
        }

        public List<string> QueryBook(string key)
        {
            IQueryable<Book> books = _context.Book;
            books = books.Where(u => u.BookName.Contains(key));
            int num = books.Count();
            List<string> bookIdList = new List<string> { };
            List<Book> booksList = books.ToList(); 
            for(int i =0;i < num; i++ )
            {
                bookIdList.Add(booksList[i].Isbn);
            }
            return bookIdList;
        }

        public string[] GetBookCollection(string user_id)
        {
            IQueryable<CollectionBook> collectionBooks = _context.CollectionBook;

            collectionBooks = collectionBooks.Where(u => u.UserId == user_id);
            string[] idArray = new string[50];
            int num = collectionBooks.Count();
            CollectionBook[] bookArray = new CollectionBook[50];
            bookArray = collectionBooks.ToArray();
            for(int i = 0;i < num; i++)
            {
                idArray[i] = bookArray[i].Isbn;
            }
            return idArray;
        }

        public int CheckBook(string isbn)
        {
            IQueryable<Book> books = _context.Book;
            books = books.Where(u => u.Isbn == isbn);
            int num = books.Count();
            return num;
        }

        public int CollectBook(string user_id,string isbn)
        {
            IQueryable<CollectionBook> collectionBooks = _context.CollectionBook;
            IQueryable<CollectionBook> isRepeat;
            collectionBooks = collectionBooks.Where(u => u.UserId == user_id);
            int valid = this.CheckBook(isbn);
            if(valid == 0)
            {
                return -2; //代表在书籍表中不存在相应的书
            }
            isRepeat = collectionBooks.Where(u => u.Isbn == isbn);
            int repeat = isRepeat.Count();
            if(repeat > 0)
            {
                return 1;//说明收藏过了
            }
            int num = collectionBooks.Count();
            if(num < 50 )
            {
                CollectionBook collect = new CollectionBook
                {
                    Isbn = isbn,
                    UserId = user_id,
                    CollectTime = DateTime.Now
                };
                _context.Add(collect);
                _context.SaveChanges();
                return 0;//说明合适
            }
            else
            {
                return -1;//说明不合适
            }
        }

        public int DecollectBook(string user_id, string isbn)
        {
            IQueryable<CollectionBook> collectionBooks = _context.CollectionBook;
            collectionBooks = collectionBooks.Where(u => u.UserId == user_id && u.Isbn == isbn);
            int num = collectionBooks.Count();
            if( num == 1 )
            {
                CollectionBook entity = collectionBooks.First();
                _context.Remove(entity);
                _context.SaveChanges();
                return 0;//没啥问题，正常取消收藏
            }
            else
            {
                return -1;//有问题
            }
        }
    }
}

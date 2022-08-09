using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace StudyPlat.Entitys
{
    public partial class ModelContext : DbContext
    {
        public ModelContext()
        {
        }

        public ModelContext(DbContextOptions<ModelContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Administrator> Administrator { get; set; }
        public virtual DbSet<Answer> Answer { get; set; }
        public virtual DbSet<Book> Book { get; set; }
        public virtual DbSet<CollectionBook> CollectionBook { get; set; }
        public virtual DbSet<CollectionCourse> CollectionCourse { get; set; }
        public virtual DbSet<CollectionQuestion> CollectionQuestion { get; set; }
        public virtual DbSet<Course> Course { get; set; }
        public virtual DbSet<CourseFromMajor> CourseFromMajor { get; set; }
        public virtual DbSet<Expert> Expert { get; set; }
        public virtual DbSet<ExplainQuestion> ExplainQuestion { get; set; }
        public virtual DbSet<FeedbackInfo> FeedbackInfo { get; set; }
        public virtual DbSet<FeedbackPosting> FeedbackPosting { get; set; }
        public virtual DbSet<FeedbackReception> FeedbackReception { get; set; }
        public virtual DbSet<GiveAnswer> GiveAnswer { get; set; }
        public virtual DbSet<HasBook> HasBook { get; set; }
        public virtual DbSet<HasExpert> HasExpert { get; set; }
        public virtual DbSet<Major> Major { get; set; }
        public virtual DbSet<Question> Question { get; set; }
        public virtual DbSet<QuestionFromBook> QuestionFromBook { get; set; }
        public virtual DbSet<QuestionFromCourse> QuestionFromCourse { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseOracle("Data source=124.220.158.211:1521/xe;User Id=admin;Password=admin");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:DefaultSchema", "ADMIN");

            modelBuilder.Entity<Administrator>(entity =>
            {
                entity.ToTable("administrator");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumbe)
                    .HasColumnName("phone_numbe")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Position)
                    .HasColumnName("position")
                    .HasMaxLength(254)
                    .IsUnicode(false);

                entity.Property(e => e.SecondaryP)
                    .HasColumnName("secondary_p")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasMaxLength(254)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Answer>(entity =>
            {
                entity.ToTable("answer");

                entity.Property(e => e.AnswerId)
                    .HasColumnName("answer_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AnswerContent)
                    .HasColumnName("answer_content")
                    .HasColumnType("CLOB");
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.Isbn)
                    .HasName("SYS_C0010321");

                entity.ToTable("book");

                entity.Property(e => e.Isbn)
                    .HasColumnName("isbn")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Author)
                    .HasColumnName("author")
                    .HasMaxLength(254)
                    .IsUnicode(false);

                entity.Property(e => e.BookName)
                    .HasColumnName("book_name")
                    .HasMaxLength(254)
                    .IsUnicode(false);

                entity.Property(e => e.Comprehension)
                    .HasColumnName("comprehension")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PublishTime)
                    .HasColumnName("publish_time")
                    .HasColumnType("NUMBER(4)");

                entity.Property(e => e.Publisher)
                    .HasColumnName("publisher")
                    .HasMaxLength(254)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CollectionBook>(entity =>
            {
                entity.HasKey(e => new { e.Isbn, e.UserId })
                    .HasName("SYS_C0010380");

                entity.ToTable("collection_book");

                entity.Property(e => e.Isbn)
                    .HasColumnName("isbn")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CollectTime)
                    .HasColumnName("collect_time")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.Note)
                    .HasColumnName("note")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.HasOne(d => d.IsbnNavigation)
                    .WithMany(p => p.CollectionBook)
                    .HasForeignKey(d => d.Isbn)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("SYS_C0010381");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CollectionBook)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("SYS_C0010382");
            });

            modelBuilder.Entity<CollectionCourse>(entity =>
            {
                entity.HasKey(e => new { e.CourseId, e.UserId })
                    .HasName("SYS_C0010371");

                entity.ToTable("collection_course");

                entity.Property(e => e.CourseId)
                    .HasColumnName("course_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CollectTime)
                    .HasColumnName("collect_time")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.Note)
                    .HasColumnName("note")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.CollectionCourse)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("SYS_C0010372");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CollectionCourse)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("SYS_C0010373");
            });

            modelBuilder.Entity<CollectionQuestion>(entity =>
            {
                entity.HasKey(e => new { e.QuestionId, e.UserId })
                    .HasName("SYS_C0010364");

                entity.ToTable("collection_question");

                entity.Property(e => e.QuestionId)
                    .HasColumnName("question_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CollectTime)
                    .HasColumnName("collect_time")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.Note)
                    .HasColumnName("note")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.CollectionQuestion)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("SYS_C0010365");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CollectionQuestion)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("SYS_C0010366");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("course");

                entity.Property(e => e.CourseId)
                    .HasColumnName("course_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Comprehension)
                    .HasColumnName("comprehension")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CourseName)
                    .HasColumnName("course_name")
                    .HasMaxLength(254)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CourseFromMajor>(entity =>
            {
                entity.HasKey(e => new { e.MajorId, e.CourseId })
                    .HasName("SYS_C0010345");

                entity.ToTable("course_from_major");

                entity.Property(e => e.MajorId)
                    .HasColumnName("major_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CourseId)
                    .HasColumnName("course_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.CourseFromMajor)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("SYS_C0010347");

                entity.HasOne(d => d.Major)
                    .WithMany(p => p.CourseFromMajor)
                    .HasForeignKey(d => d.MajorId)
                    .HasConstraintName("SYS_C0010346");
            });

            modelBuilder.Entity<Expert>(entity =>
            {
                entity.ToTable("expert");

                entity.Property(e => e.ExpertId)
                    .HasColumnName("expert_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ExpertName)
                    .HasColumnName("expert_name")
                    .HasMaxLength(254)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ExplainQuestion>(entity =>
            {
                entity.HasKey(e => new { e.QuestionId, e.AnswerId })
                    .HasName("SYS_C0010385");

                entity.ToTable("explain_question");

                entity.Property(e => e.QuestionId)
                    .HasColumnName("question_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AnswerId)
                    .HasColumnName("answer_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreateTime)
                    .HasColumnName("create_time")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.MostCollected).HasColumnName("most_collected");

                entity.Property(e => e.Official).HasColumnName("official");

                entity.HasOne(d => d.Answer)
                    .WithMany(p => p.ExplainQuestion)
                    .HasForeignKey(d => d.AnswerId)
                    .HasConstraintName("SYS_C0010387");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.ExplainQuestion)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("SYS_C0010386");
            });

            modelBuilder.Entity<FeedbackInfo>(entity =>
            {
                entity.HasKey(e => e.FeedbackId)
                    .HasName("SYS_C0010339");

                entity.ToTable("feedback_info");

                entity.Property(e => e.FeedbackId)
                    .HasColumnName("feedback_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Content)
                    .HasColumnName("content")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.PostTime)
                    .HasColumnName("post_time")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.ProblemType).HasColumnName("problem_type");

                entity.Property(e => e.QuestionId)
                    .HasColumnName("question_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Replay)
                    .HasColumnName("replay")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.FeedbackInfo)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("SYS_C0010352");
            });

            modelBuilder.Entity<FeedbackPosting>(entity =>
            {
                entity.HasKey(e => e.FeedbackId)
                    .HasName("SYS_C0010388");

                entity.ToTable("feedback_posting");

                entity.Property(e => e.FeedbackId)
                    .HasColumnName("feedback_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Feedback)
                    .WithOne(p => p.FeedbackPosting)
                    .HasForeignKey<FeedbackPosting>(d => d.FeedbackId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("SYS_C0010390");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.FeedbackPosting)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("SYS_C0010389");
            });

            modelBuilder.Entity<FeedbackReception>(entity =>
            {
                entity.HasKey(e => e.FeedbackId)
                    .HasName("SYS_C0010391");

                entity.ToTable("feedback_reception");

                entity.Property(e => e.FeedbackId)
                    .HasColumnName("feedback_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AdministratorId)
                    .HasColumnName("administrator_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Read).HasColumnName("read");

                entity.Property(e => e.ReceptTime)
                    .HasColumnName("recept_time")
                    .HasColumnType("TIMESTAMP(6)");

                entity.HasOne(d => d.Administrator)
                    .WithMany(p => p.FeedbackReception)
                    .HasForeignKey(d => d.AdministratorId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("SYS_C0010393");

                entity.HasOne(d => d.Feedback)
                    .WithOne(p => p.FeedbackReception)
                    .HasForeignKey<FeedbackReception>(d => d.FeedbackId)
                    .HasConstraintName("SYS_C0010392");
            });

            modelBuilder.Entity<GiveAnswer>(entity =>
            {
                entity.HasKey(e => new { e.ExpertId, e.AnswerId })
                    .HasName("SYS_C0010320");

                entity.ToTable("give_answer");

                entity.Property(e => e.ExpertId)
                    .HasColumnName("expert_id")
                    .HasMaxLength(254)
                    .IsUnicode(false);

                entity.Property(e => e.AnswerId)
                    .HasColumnName("answer_id")
                    .HasMaxLength(254)
                    .IsUnicode(false);

                entity.Property(e => e.AdditionDate)
                    .HasColumnName("addition_date")
                    .HasColumnType("DATE");

                entity.HasOne(d => d.Answer)
                    .WithMany(p => p.GiveAnswer)
                    .HasForeignKey(d => d.AnswerId)
                    .HasConstraintName("SYS_C0010354");

                entity.HasOne(d => d.Expert)
                    .WithMany(p => p.GiveAnswer)
                    .HasForeignKey(d => d.ExpertId)
                    .HasConstraintName("SYS_C0010353");
            });

            modelBuilder.Entity<HasBook>(entity =>
            {
                entity.HasKey(e => new { e.CourseId, e.Isbn })
                    .HasName("SYS_C0010341");

                entity.ToTable("has_book");

                entity.Property(e => e.CourseId)
                    .HasColumnName("course_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Isbn)
                    .HasColumnName("isbn")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AdditionDate)
                    .HasColumnName("addition_date")
                    .HasColumnType("DATE");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.HasBook)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("SYS_C0010342");

                entity.HasOne(d => d.IsbnNavigation)
                    .WithMany(p => p.HasBook)
                    .HasForeignKey(d => d.Isbn)
                    .HasConstraintName("SYS_C0010344");
            });

            modelBuilder.Entity<HasExpert>(entity =>
            {
                entity.HasKey(e => new { e.ExpertId, e.MajorId })
                    .HasName("SYS_C0010349");

                entity.ToTable("has_expert");

                entity.Property(e => e.ExpertId)
                    .HasColumnName("expert_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MajorId)
                    .HasColumnName("major_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Expert)
                    .WithMany(p => p.HasExpert)
                    .HasForeignKey(d => d.ExpertId)
                    .HasConstraintName("SYS_C0010350");

                entity.HasOne(d => d.Major)
                    .WithMany(p => p.HasExpert)
                    .HasForeignKey(d => d.MajorId)
                    .HasConstraintName("SYS_C0010351");
            });

            modelBuilder.Entity<Major>(entity =>
            {
                entity.ToTable("major");

                entity.Property(e => e.MajorId)
                    .HasColumnName("major_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MajorName)
                    .HasColumnName("major_name")
                    .HasMaxLength(254)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("question");

                entity.Property(e => e.QuestionId)
                    .HasColumnName("question_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PostTime)
                    .HasColumnName("post_time")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.QuestionStem)
                    .HasColumnName("question_stem")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Source)
                    .HasColumnName("source")
                    .HasMaxLength(254)
                    .IsUnicode(false);

                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<QuestionFromBook>(entity =>
            {
                entity.HasKey(e => e.QuestionId)
                    .HasName("SYS_C0010315");

                entity.ToTable("question_from_book");

                entity.Property(e => e.QuestionId)
                    .HasColumnName("question_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Isbn)
                    .IsRequired()
                    .HasColumnName("isbn")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Page).HasColumnName("page");

                entity.HasOne(d => d.IsbnNavigation)
                    .WithMany(p => p.QuestionFromBook)
                    .HasForeignKey(d => d.Isbn)
                    .HasConstraintName("SYS_C0010357");

                entity.HasOne(d => d.Question)
                    .WithOne(p => p.QuestionFromBook)
                    .HasForeignKey<QuestionFromBook>(d => d.QuestionId)
                    .HasConstraintName("SYS_C0010356");
            });

            modelBuilder.Entity<QuestionFromCourse>(entity =>
            {
                entity.HasKey(e => new { e.QuestionId, e.CourseId })
                    .HasName("SYS_C0010317");

                entity.ToTable("question_from_course");

                entity.Property(e => e.QuestionId)
                    .HasColumnName("question_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CourseId)
                    .HasColumnName("course_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.QuestionFromCourse)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("SYS_C0010361");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.QuestionFromCourse)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("SYS_C0010360");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumbe)
                    .HasColumnName("phone_numbe")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasMaxLength(254)
                    .IsUnicode(false);

                entity.Property(e => e.UserType).HasColumnName("user_type");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EduClass.Entities
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class EduClassContext : DbContext
    {
        public EduClassContext()
            : base("name=EduClassContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<Board> Boards { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Test> Tests { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<QuestionOption> QuestionOptions { get; set; }
        public virtual DbSet<QuestionType> QuestionTypes { get; set; }
        public virtual DbSet<Response> Responses { get; set; }
        public virtual DbSet<Key> Keys { get; set; }
        public virtual DbSet<Calendar> Calendars { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<EventType> EventTypes { get; set; }
        public virtual DbSet<Page> Pages { get; set; }
        public virtual DbSet<Alert> Alerts { get; set; }
        public virtual DbSet<Mail> Mails { get; set; }
        public virtual DbSet<Avatar> Avatars { get; set; }
        public virtual DbSet<PostType> PostTypes { get; set; }
        public virtual DbSet<Reply> Replies { get; set; }
    }
}

﻿namespace personal_pages
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class personal_pageEntities : DbContext
    {
        public personal_pageEntities()
            : base("name=personal_pageEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<ClassBook> ClassBooks { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Departament> Departaments { get; set; }
        public virtual DbSet<Education_Form> Education_Form { get; set; }
        public virtual DbSet<ELMAH_Error> ELMAH_Error { get; set; }
        public virtual DbSet<Faculty> Faculties { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<University> Universities { get; set; }
        public virtual DbSet<User> Users { get; set; }
    
        public virtual ObjectResult<string> ELMAH_GetErrorsXml(string application, Nullable<int> pageIndex, Nullable<int> pageSize, ObjectParameter totalCount)
        {
            var applicationParameter = application != null ?
                new ObjectParameter("Application", application) :
                new ObjectParameter("Application", typeof(string));
    
            var pageIndexParameter = pageIndex.HasValue ?
                new ObjectParameter("PageIndex", pageIndex) :
                new ObjectParameter("PageIndex", typeof(int));
    
            var pageSizeParameter = pageSize.HasValue ?
                new ObjectParameter("PageSize", pageSize) :
                new ObjectParameter("PageSize", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("ELMAH_GetErrorsXml", applicationParameter, pageIndexParameter, pageSizeParameter, totalCount);
        }
    
        public virtual ObjectResult<string> ELMAH_GetErrorXml(string application, Nullable<System.Guid> errorId)
        {
            var applicationParameter = application != null ?
                new ObjectParameter("Application", application) :
                new ObjectParameter("Application", typeof(string));
    
            var errorIdParameter = errorId.HasValue ?
                new ObjectParameter("ErrorId", errorId) :
                new ObjectParameter("ErrorId", typeof(System.Guid));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("ELMAH_GetErrorXml", applicationParameter, errorIdParameter);
        }
    
        public virtual int ELMAH_LogError(Nullable<System.Guid> errorId, string application, string host, string type, string source, string message, string user, string allXml, Nullable<int> statusCode, Nullable<System.DateTime> timeUtc)
        {
            var errorIdParameter = errorId.HasValue ?
                new ObjectParameter("ErrorId", errorId) :
                new ObjectParameter("ErrorId", typeof(System.Guid));
    
            var applicationParameter = application != null ?
                new ObjectParameter("Application", application) :
                new ObjectParameter("Application", typeof(string));
    
            var hostParameter = host != null ?
                new ObjectParameter("Host", host) :
                new ObjectParameter("Host", typeof(string));
    
            var typeParameter = type != null ?
                new ObjectParameter("Type", type) :
                new ObjectParameter("Type", typeof(string));
    
            var sourceParameter = source != null ?
                new ObjectParameter("Source", source) :
                new ObjectParameter("Source", typeof(string));
    
            var messageParameter = message != null ?
                new ObjectParameter("Message", message) :
                new ObjectParameter("Message", typeof(string));
    
            var userParameter = user != null ?
                new ObjectParameter("User", user) :
                new ObjectParameter("User", typeof(string));
    
            var allXmlParameter = allXml != null ?
                new ObjectParameter("AllXml", allXml) :
                new ObjectParameter("AllXml", typeof(string));
    
            var statusCodeParameter = statusCode.HasValue ?
                new ObjectParameter("StatusCode", statusCode) :
                new ObjectParameter("StatusCode", typeof(int));
    
            var timeUtcParameter = timeUtc.HasValue ?
                new ObjectParameter("TimeUtc", timeUtc) :
                new ObjectParameter("TimeUtc", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ELMAH_LogError", errorIdParameter, applicationParameter, hostParameter, typeParameter, sourceParameter, messageParameter, userParameter, allXmlParameter, statusCodeParameter, timeUtcParameter);
        }
    
        public virtual int sp_alterdiagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_alterdiagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_creatediagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_creatediagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_dropdiagram(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_dropdiagram", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagramdefinition_Result> sp_helpdiagramdefinition(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagramdefinition_Result>("sp_helpdiagramdefinition", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagrams_Result> sp_helpdiagrams(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagrams_Result>("sp_helpdiagrams", diagramnameParameter, owner_idParameter);
        }
    
        public virtual int sp_renamediagram(string diagramname, Nullable<int> owner_id, string new_diagramname)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var new_diagramnameParameter = new_diagramname != null ?
                new ObjectParameter("new_diagramname", new_diagramname) :
                new ObjectParameter("new_diagramname", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_renamediagram", diagramnameParameter, owner_idParameter, new_diagramnameParameter);
        }
    
        public virtual int sp_upgraddiagrams()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_upgraddiagrams");
        }
    }
}

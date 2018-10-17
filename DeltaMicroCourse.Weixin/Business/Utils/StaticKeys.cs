using System.Collections.Generic;
using TheSite.Models;
using System.Linq;

namespace Business
{

   public static class StaticKeys
   {

      public enum SourceEnum { fromKeys = 0, fromDB = 1, fromOther = 2 }

      public const string Course = "微课程名";
      public const string Subejct = "学科";
      public const string School = "学校";
      public const string Author = "作者";
      public const string Grade = "年级";
      public const string Active = "项目";

      public const int DefaultNumber = 0;
      public const int CourseNameNumber = (int)SearchOptionType.CourseName;
      public const int SubjectNumber = (int)SearchOptionType.Subject;
      public const int SchoolNumber = (int)SearchOptionType.School;
      public const int GradeNumber = (int)SearchOptionType.Grade;
      public const int AuthorNumber = (int)SearchOptionType.Author;
      public const int ActiveNumber = (int)SearchOptionType.Active;

      public static Dictionary<int, string> Categories = new Dictionary<int, string>
        {
            {CourseNameNumber, Course },
            {SubjectNumber,Subejct},
            {SchoolNumber,School},
            {GradeNumber,Grade},
            {AuthorNumber,Author},
            {ActiveNumber,Active},
			//{-3,"区域"},
		};

      public static Dictionary<long, Dictionary<long, string>> CategoryItems = new Dictionary<long, Dictionary<long, string>>
        {
            {
                SubjectNumber,PickListHelper.GetItemsByPKID(ThisApp.PLKey_ResourceSubject)
            },
            {
                GradeNumber, PickListHelper.GetItemsByPKID(ThisApp.PLKey_ResourceGrade)
            },
            {
                ActiveNumber, ActiveHelper.All==null? new Dictionary<long, string>() : ActiveHelper.All.ToDictionary(x=>x.ActiveId,y=>y.ActiveName)
            }
        };

      public static Dictionary<string, SourceEnum> SourceKeys = new Dictionary<string, SourceEnum>
        {
            { Course,SourceEnum.fromDB},
            { Subejct,SourceEnum.fromKeys},
            { School,SourceEnum.fromDB},
            { Grade,SourceEnum.fromKeys},
            { Author,SourceEnum.fromDB },
            { Active,SourceEnum.fromKeys }
        };

   }

}
using System.Collections.Generic;
using TheSite.Models;

namespace Business
{

   public static class StaticKeys
    {

        public enum SourceEnum { fromKeys = 0, fromDB = 1, fromOther = 2 }

        public const string Course = "微课程名";
        public const string Subejct = "学科";
        public const string School= "学校";
        public const string Author = "作者";
        public const string Grade = "年级";

	     public const int DefaultNumber = 0;
		  public const int CourseNameNumber = (int)SearchOptionType.CourseName;
        public const int SubjectNumber = (int)SearchOptionType.Subject;
        public const int SchoolNumber = (int)SearchOptionType.School;
        public const int GradeNumber = (int)SearchOptionType.Grade;
        public const int AuthorNumber = (int)SearchOptionType.Author;


        public static Dictionary<int, string> Categories = new Dictionary<int, string>
        {
            {CourseNameNumber, Course },
            {SubjectNumber,Subejct},
            {SchoolNumber,School},
            {GradeNumber,Grade},
            {AuthorNumber,Author},
			//{-3,"区域"},
		};

        public static Dictionary<int, Dictionary<int, string>> CategoryItems = new Dictionary<int, Dictionary<int, string>>
        {
            {
                SubjectNumber, new Dictionary<int ,string>
                {
                    { 2, "语文" }, { 3, "英语" }, {4, "数学" }, { 5, "政治" }, { 6, "思想品德" } ,
                    { 74, "社会" }, { 8, "历史" }, {9, "地理" }, { 10, "物理" }, {11, "化学" },
                    { 12, "生物" }, { 13, "科学" }, {14, "劳动与技术教育" }, { 15, "信息技术教育" }, {16, "艺术" },
                }
            }
            ,
            {
                GradeNumber, new Dictionary<int ,string>
                {
                    { 85, "一年级" }, { 86, "二年级" }, {87, "三年级" }, { 88, "四年级" }, { 89, "五年级" } , { 90, "六年级" },
                    { 91, "初一" }, { 92, "初二" }, {93, "初三" },
                    { 94, "高一" }, {95, "高二" },{ 96, "高三" },
                }
            }
        };

        public static Dictionary<string, SourceEnum> SourceKeys = new Dictionary<string, SourceEnum>
        {
            { Course,SourceEnum.fromDB},
            { Subejct,SourceEnum.fromKeys},
            { School,SourceEnum.fromDB},
            { Grade,SourceEnum.fromKeys},
			{ Author,SourceEnum.fromDB }
        };

    }

}

//17, 1, 音乐, discipline, 0, 
//18, 1, 美术, discipline, 0, 
//19, 1, 体育, discipline, 0, 
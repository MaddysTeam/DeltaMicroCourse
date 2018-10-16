using Business;
using System;

namespace TheSite.Models
{

	public class CourseDetailsViewModel
	{

		//public CourseInfo CourseInfo { get; set; }

	}


   public class CourseViewModel
   {
      public string ImagePath { get; set; }
      public string VideoPath { get; set; }
      public string LinkUrl { get; set; }
      public bool IsFavorite { get; set; }
      public string Name { get; set; }
      public string TeacherName { get; set; }
      public int PlayCount { get; set; }
      public int PraiseCount { get; set; }
      public string SchoolName { get; set; }
      public int VideoId { get; set; }
      public DateTime CreateDate { get; set; }
   }


	public class SearchOption
	{

		public SearchOptionType SearchType { get; set; } = SearchOptionType.CourseName;

		public string SearchTerm { get; set; }

		public string CurrentOpenId { get; set; }

		public int CategoryItemId { get; set; }

		public int CategoryId { get; set; } = (int)SearchOptionType.CourseName;

	}


	public enum SearchOptionType
	{

		//最热微课排行，依据课程视频点击量排序
		HotList = 0,

		//微课得票排行，依据课程视频点赞量排序
		PraiseList = 1,

		//地区最新微课，依据选中地区和时间顺序排序
		LatestListByRegion = 2,

		//根据微课程名搜索
		CourseName = 3,

		//搜索我的收藏
		MyFavorite = 4,

		//根据科目Id搜索
		Subject = 5,

		//根据学校名字搜索
		School = 6,

		//根据年级搜索
		Grade = 7,

		//根据作者名字搜索
		Author = 8,

	}

}
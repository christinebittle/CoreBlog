﻿namespace Blog.Models
{
    public class Tag
    {
        //describes a tag in our blog database

        public int TagId { get; set; }

        public string TagName { get; set; }

        public string TagColor { get; set; }

        // could have other fields
        // ie. TagPopularity, TagDate, TagDescription, TagImage

    }
}

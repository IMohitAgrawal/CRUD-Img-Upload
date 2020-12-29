using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CRUD_Img_Upload.Models
{
    public class ImageCrudClass
    {
        [Key]
        public int Imgid { get; set; } //Here the Property name is column name.
        public string Imgname { get; set; } //Here the Property name is column name.
        public String Imgpath { get; set; } //Here the Property name is column name.
    }
}

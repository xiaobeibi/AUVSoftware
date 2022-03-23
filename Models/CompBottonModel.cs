using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AUVSoftware.Models
{
    public class CompBottonModel : ObservableObject
    {
        public CompBottonModel()
        {
            //构造函数
        }

        private String content;
        public String Content
        {
            get { return content; }
            set { content = value; RaisePropertyChanged(() => Content); }
        }


        private Boolean isCheck;
        public Boolean IsCheck
        {
            get { return isCheck; }
            set { isCheck = value; RaisePropertyChanged(() => IsCheck); }
        }
    }
}

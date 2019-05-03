using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels
{
    public enum FrmType
    {

        Write, Modify, Reply, Delete

    }

    public static class FrmTypeExtensions
    {
        public static string ToFriendlyString(this FrmType fType)
        {
            string r = "";

            switch (fType)
            {
                case FrmType.Modify:
                    r = "수정 페이지";
                    break;
                case FrmType.Reply:
                    r = "답변 페이지";
                    break;
                case FrmType.Delete:
                    r = "삭제 페이지";
                    break;
                default:
                    r = "등록 페이지";
                    break;
            }

            return r;

        }
    }
}

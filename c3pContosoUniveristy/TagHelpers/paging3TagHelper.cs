using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace c2ContosoUniveristy.TagHelpers
{
    /// <summary>
    /// 페이징 헬퍼(dnn-paging-helper)
    /// </summary>
    public class paging3TagHelper : TagHelper
    {

        /// <summary>
        /// 기본 리스트면 false, 검색 결과에 대한 페이징 리스트면 true
        /// </summary>
        public bool SearchMode { get; set; } = false;
        /// <summary>
        /// 검색할 필드: Name, Title, Content
        /// </summary>
        public string SearchField { get; set; }
        /// <summary>
        /// 검색할 내용
        /// </summary>
        public string SearchQuery { get; set; }

        /// <summary>
        /// 현재 보여줄 페이지 인덱스: 0, 1, 2
        /// </summary>
        public int PageIndex { get; set; } = 0;
        /// <summary>
        /// 총 페이지 개수 (PageCount = > TotalPage로 변경)
        /// </summary>
        public int TotalPage { get; set; }
        /// <summary>
        /// 한 페이지에 보여줄 아티클 개수
        /// </summary>
        public int PageSize { get; set; } = 10;
        /// 한 페이지에 보여줄 페이져 개수(국헌 추가)
        /// </summary>
        public int PageCounter { get; set; } = 10;

        /// <summary>
        /// 페이징 헬퍼가 실행될 URL
        /// </summary>
        public string Url { get; set; }

        private int _RecordCount;
        /// <summary>
        /// 총 레코드 수
        /// </summary>

        /// <summary>
        /// 페이징 헬퍼가 실행될 URL
        /// </summary>
        public string PageEtc { get; set; }


        public int RecordCount
        {
            get { return _RecordCount; }
            set
            {
                _RecordCount = value;
                TotalPage = ((_RecordCount - 1) / PageSize) + 1; // 계산식
            }
        }

        public override void Process(
            TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "ul";
            output.Attributes.Add("class", "pagination pagination-sm");

            if (PageIndex == 0)
            {
                PageIndex = 1;
            }

            int i = 0;

            string strPage = "";

            PageEtc = string.IsNullOrEmpty(PageEtc) ? "" : "&" + PageEtc;



            #region 처음 페이지로 이동
            if (PageIndex > 1)
            {

                if (!SearchMode)
                {
                    strPage += "<li><a href =\""
                          + Url + "?Page=1" + PageEtc + "\">&laquo;</a></li>";
                }
                else
                {
                    strPage += "<li><a href=\""
                        + Url + "?Page=1" + PageEtc 
                        + "&SearchField=" + SearchField
                        + "&SearchQuery=" + SearchQuery + "\">&laquo;</a></li>";

                }

            }
            else
            {

                strPage += "<li class='disabled'><a>&laquo;</a></li>";
            }
            #endregion

            #region 이전화살표
            if (PageIndex > PageCounter)
            {
                if (!SearchMode)
                {
                    strPage += "<li><a href=\"" + Url + "?Page="
                        + Convert.ToString(((PageIndex - 1) / (int)PageCounter) * PageCounter)
                        + PageEtc + "\">&larr;</a></li>";
                }
                else
                {
                    strPage += "<li><a href=\"" + Url + "?Page="
                        + Convert.ToString(((PageIndex - 1) / (int)PageCounter) * PageCounter) + PageEtc 
                        + "&SearchField=" + SearchField
                        + "&SearchQuery=" + SearchQuery + "\">&larr;</a></li>";
                }
            }
            else
            {
                strPage += "<li class='disabled'><a>&larr;</a></li>";
            }
            #endregion


            #region 페이지 번호 및 다음 화살표
            for (i = (((PageIndex - 1) / (int)PageCounter) * PageCounter + 1);
                    i <= ((((PageIndex - 1) / (int)PageCounter) + 1) * PageCounter); i++)
            {
                if (i > TotalPage)
                {
                    break;
                }
                if (i == PageIndex)
                {

                    strPage += " <li class='active'><a>"
                        + i.ToString() + "</a></li>";

                }
                else
                {
                    if (!SearchMode)
                    {
                        strPage += "<li><a href=\"" + Url + "?Page="
                            + i.ToString() + PageEtc + "\">" + i.ToString() + "</a></li>";
                    }
                    else
                    {
                        strPage += "<li><a href=\"" + Url + "?Page="
                            + i.ToString() + PageEtc + "&SearchField=" + SearchField
                            + "&SearchQuery=" + SearchQuery + "\">"
                            + i.ToString() + "</a></li>";
                    }
                }
            }
            #endregion

            //국헌 생각으로는 i++되어 오므로 -1을 해줘야함
            if ((i - 1) < TotalPage)
            {
                if (!SearchMode)
                {
                    strPage += "<li><a href=\"" + Url + "?Page="
                        + Convert.ToString(((PageIndex - 1) / (int)PageCounter) * PageCounter + (PageCounter + 1))
                        + PageEtc + "\">&rarr;</a></li>";
                }
                else
                {
                    strPage += "<li><a href=\"" + Url + "?Page="
                        + Convert.ToString(((PageIndex - 1) / (int)PageCounter) * PageCounter + (PageCounter + 1))
                        + PageEtc
                        + "&SearchField=" + SearchField
                        + "&SearchQuery=" + SearchQuery + "\">&rarr;</a></li>";
                }
            }
            else
            {
                strPage += "<li class='disabled'><a>&rarr;</a></li>";
            }

            #region 마지막 페이지로 이동
            if (PageIndex < TotalPage)
            {

                if (!SearchMode)
                {
                    strPage += "<li><a href=\"" + Url + "?Page="
                        + TotalPage.ToString()
                        + PageEtc + "\">&raquo;</a></li>";
                }
                else
                {
                    strPage += "<li><a href=\"" + Url + "?Page="
                        + TotalPage.ToString()
                        + PageEtc
                        + "&SearchField=" + SearchField
                        + "&SearchQuery=" + SearchQuery + "\">&raquo;</a></li>";

                }

            }
            else
            {

                strPage += "<li class='disabled'><a>&raquo;</a></li>";
            }
            #endregion

            output.Content.AppendHtml(strPage);

        }
    }
}

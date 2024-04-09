using System.Collections.Generic;
using LSSERVICEPROVIDERLib;
using MSXML;

namespace XmlService
{
    public class ResultEntryXmlHandler : BaseXmlHandler
    {
        private IXMLDOMElement objLoad;

        public ResultEntryXmlHandler(INautilusServiceProvider sp)
            : base(sp)
        {

        }
        public ResultEntryXmlHandler(INautilusServiceProvider sp, string savedXmlName)
            : base(sp, savedXmlName)
        {

        }

        public void CreateResultEntryXml(long testId, long ResultId, string resultValue)
        {
            CreateResultEntryXml(testId.ToString());
            AddResultEntryElem(ResultId.ToString(), resultValue);

        }
        public void CreateDefaultResultEntryXml(long testId, long ResultId)
        {
            CreateResultEntryXml(testId.ToString());
            AddDefaultReElement(ResultId.ToString());
        }
        public void CreateResultEntryXmlWithAliquot(long aliquotId, Dictionary<string, string> dictionary)
        {
            CreateResultEntryXmlWithAliquot(aliquotId.ToString());
            
            foreach (KeyValuePair<string, string> keyValuePair in dictionary)
            {
                AddResultEntryElem(keyValuePair.Key, keyValuePair.Value);
            }
        }


        private void CreateResultEntryXml(string testId)
        {
            objDoc = new DOMDocument();

            IXMLDOMElement objResultRequest = objDoc.createElement("result-request");
            objDoc.appendChild(objResultRequest);

            objLoad = objDoc.createElement("load");
            objLoad.setAttribute("entity", "TEST");

            objLoad.setAttribute("id", testId);
            objLoad.setAttribute("mode", "entry");

            objResultRequest.appendChild(objLoad);

        }
        private void CreateResultEntryXmlWithAliquot(string aliquotId)
        {
            objDoc = new DOMDocument();

            IXMLDOMElement objResultRequest = objDoc.createElement("result-request");
            objDoc.appendChild(objResultRequest);

            objLoad = objDoc.createElement("load");
            objLoad.setAttribute("entity", "ALIQUOT");

            objLoad.setAttribute("id", aliquotId);
            objLoad.setAttribute("mode", "entry");

            objResultRequest.appendChild(objLoad);

        }
        /// <summary>
        /// Add element result-default to xml
        /// </summary>
        /// <param name="resultId">result entry id</param>
        /// <returns>new element</returns>
        private void AddDefaultReElement(string resultId)
        {
            IXMLDOMElement objResultEntryElem = objDoc.createElement("result-default");
            objResultEntryElem.setAttribute("result-id", resultId);
            objLoad.appendChild(objResultEntryElem);
        }

        /// <summary>
        /// Add element result-entry to xml
        /// </summary>
        /// <param name="resultId">result entry id</param>
        /// <param name="value">result entry value</param>
        private void AddResultEntryElem(string resultId, string value)
        {
            IXMLDOMElement objResultEntryElem = objDoc.createElement("result-entry");
            objResultEntryElem.setAttribute("result-id", resultId);
            objResultEntryElem.setAttribute("original-result", value);
            objLoad.appendChild(objResultEntryElem);

        }
    }
}
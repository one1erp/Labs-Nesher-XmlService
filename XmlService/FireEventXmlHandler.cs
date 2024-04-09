using LSSERVICEPROVIDERLib;
using MSXML;

namespace XmlService
{
    public class FireEventXmlHandler : BaseXmlHandler
    {
        public FireEventXmlHandler(INautilusServiceProvider sp) : base(sp)
        {
        }
        public FireEventXmlHandler(INautilusServiceProvider sp, string savedXmlName)
            : base(sp, savedXmlName)
        {

        }

        /// <summary>
        /// Create fire event xml
        /// </summary>
        /// <param name="tableName">Name of Entity</param>
        /// <param name="entityId">Entity ID</param>
        /// <param name="eventName">Name of event to run</param>
        public void CreateFireEventXml(string tableName, long entityId, string eventName)
        {


            objDoc = new DOMDocument();

            //Creates lims request element
            IXMLDOMElement objLimsElem = objDoc.createElement("lims-request");
            objDoc.appendChild(objLimsElem);

            // Creates login request element
            IXMLDOMElement objLoginElem = objDoc.createElement("login-request");
            objLimsElem.appendChild(objLoginElem);

            // Creates Entity element
            IXMLDOMElement objEntityElem = objDoc.createElement(tableName);
            objLoginElem.appendChild(objEntityElem);


            // Creates   find-by-name element
            IXMLDOMElement objFindByNameElem = objDoc.createElement("find-by-id");
            objEntityElem.appendChild(objFindByNameElem);
            objFindByNameElem.text = entityId.ToString();


            //Creates fire-event element
            IXMLDOMElement objFireEvent = objDoc.createElement("fire-event");
            objEntityElem.appendChild(objFireEvent);
            objFireEvent.text = eventName;


        }


    }
}

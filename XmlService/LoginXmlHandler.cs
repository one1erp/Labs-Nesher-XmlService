using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSSERVICEPROVIDERLib;
using MSXML;

namespace XmlService
{
    public class LoginXmlHandler : BaseXmlHandler
    {

        private IXMLDOMElement tableElem;



        public LoginXmlHandler(INautilusServiceProvider sp)
            : base(sp)
        {


        }

        public LoginXmlHandler(INautilusServiceProvider sp, string savedXmlName)
            : base(sp, savedXmlName)
        {

        }

        /// <summary>
        /// Create login xml
        /// </summary>
        /// <param name="tableName">Name of entity to login</param>
        /// <param name="workflowName">Name of workflow to creates entity </param>
        public void CreateLoginXml(string tableName, string workflowName)
        {

            objDoc = new DOMDocument();

            //Creates lims request element
            var objLimsElem = objDoc.createElement("lims-request");
            objDoc.appendChild(objLimsElem);

            // Creates login request element
            var objLoginElem = objDoc.createElement("login-request");
            objLimsElem.appendChild(objLoginElem);

            // Creates tableName element
            tableElem = objDoc.createElement(tableName);
            objLoginElem.appendChild(tableElem);

            // Creates create by workflow element
            var objCreateByWorkflowElem = objDoc.createElement("create-by-workflow");
            tableElem.appendChild(objCreateByWorkflowElem);


            //'creates workflow-name element
            var objMemberName = objDoc.createElement("workflow-name");
            objCreateByWorkflowElem.appendChild(objMemberName);
            objMemberName.text = workflowName;



        }

        /// <summary>
        /// Create Login xml under other entity
        /// </summary>
        /// <param name="baseTable">Name of parent entity</param>
        /// <param name="baseObjIdentity">Parent ID</param>
        /// <param name="childTable">Name of entity to login</param>
        /// <param name="workFlowName">Name of workflow to creates entity</param>
        /// <param name="findBy">find by parent</param>
        public void CreateLoginChildXml(string baseTable, string baseObjIdentity, string childTable, string workFlowName, FindBy findBy)
        {
            objDoc = new DOMDocument();
            var objLimsElem = objDoc.createElement("lims-request");
            objDoc.appendChild(objLimsElem);

            // Creates login request element
            var objLoginElem = objDoc.createElement("login-request");
            objLimsElem.appendChild(objLoginElem);


            var entityBaseName = objDoc.createElement(baseTable);
            objLoginElem.appendChild(entityBaseName);


            AddFindByElement(entityBaseName, baseObjIdentity, findBy);

            tableElem = objDoc.createElement(childTable);
            entityBaseName.appendChild(tableElem);


            var createByWfElem = objDoc.createElement("create-by-workflow");
            tableElem.appendChild(createByWfElem);

            var WFNameElem = objDoc.createElement("workflow-name");
            createByWfElem.appendChild(WFNameElem);
            WFNameElem.text = workFlowName;

        }

        /// <summary>
        /// Add property to login xml
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        public void AddProperties(string propertyName, string propertyValue)
        {

            AddPropertiesToXml(propertyName, propertyValue);
        }

        /// <summary>
        /// Create property element
        /// </summary>
        /// <param name="propertyName">Name of property in DB</param>
        /// <param name="propertyValue">Property value</param>     
        private void AddPropertiesToXml(string propertyName, string propertyValue)
        {

            IXMLDOMElement objpropertyName = objDoc.createElement(propertyName);
            tableElem.appendChild(objpropertyName);
            objpropertyName.text = propertyValue;

        }

        /// <summary>
        /// Create find by element
        /// </summary>
        /// <param name="entityBaseName"></param>
        /// <param name="baseObjIdentity"></param>
        /// <param name="findBy"></param>
        private void AddFindByElement(IXMLDOMElement entityBaseName, string baseObjIdentity, FindBy findBy)
        {
            if (findBy == FindBy.Id)
            {
                var findByElem = objDoc.createElement("find-by-id");
                entityBaseName.appendChild(findByElem);
                findByElem.text = baseObjIdentity;
            }
            else if (findBy == FindBy.Name)
            {
                var findByElem = objDoc.createElement("find-by-name");
                entityBaseName.appendChild(findByElem);
                findByElem.text = baseObjIdentity;
            }
        }

      
    }
}

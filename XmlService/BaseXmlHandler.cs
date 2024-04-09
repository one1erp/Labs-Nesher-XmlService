using System;
using System.IO;
using System.Linq;
using Common;
using DAL;
using LSSERVICEPROVIDERLib;
using MSXML;

namespace XmlService
{
    public abstract class BaseXmlHandler
    {
        protected DOMDocument objDoc;
        protected DOMDocument objRes;
        private readonly INautilusProcessXML _processXml;
        private readonly string path;
        private bool _succes;
        private string _response;
        private string fileName = "";


        protected BaseXmlHandler(INautilusServiceProvider sp)
        {

            //Get xml processor
            _processXml = Utils.GetXmlProcessor(sp);
            //Get nautilus user
            var ntlsUser = Utils.GetNautilusUser(sp);
            //Get workstation of current user
            var workstationId = ntlsUser.GetWorkstationId();
            try
            {


                var dal = new DataLayer();
                dal.Connect();
                //Get phrase of locations
                var phraseH = dal.GetPhraseByName("Location folders");
                //Get location of xml documents
                var phrase = phraseH.PhraseEntries.FirstOrDefault(x => x.PhraseDescription == "Xml Documents");
                if (phrase != null)
                {
                    //Set path for each workstation
                    path = phrase.PhraseName + "\\workstation_" + workstationId + "\\";
                }
                dal.Close();
            }
            catch (Exception exception)
            {

                Logger.WriteLogFile(exception);
            }

        }
        protected BaseXmlHandler(INautilusServiceProvider sp, string fileName)
            : this(sp)
        {
            this.fileName = fileName;
        }
        /// <summary>
        /// Run xml
        /// </summary>
        /// <returns>Success</returns>
        /// 

        public bool ProcssXml()
        {
            objRes = new DOMDocument();
            _response = _processXml.ProcessXMLWithResponse(objDoc, objRes);
            // _response = _processXml.ProcessXML(objDoc);
            string suffix = ".xml";
            string savePath = null;
            try
            {
                var directoryPath = GetDirectoryPath();
                if (directoryPath != null)
                {
                    //     for test
                    //    objDoc.save(@"C:\temp\" + "Doc" + (DateTime.Now.ToString().MakeSafeFilename('_')) + ".xml");
                    //    objRes.save(@"C:\temp\" + "Res" + (DateTime.Now.ToString().MakeSafeFilename('_')) + ".xml");

                    string ut = DateTime.UtcNow.ToString("yyyy-MM-dd HH-mm-ss-fff");


                    objDoc.save(directoryPath + "Doc_" + fileName.MakeSafeFilename('_') + (ut) + suffix);
                    savePath = (directoryPath + "Res_" + fileName.MakeSafeFilename('_') + (ut));
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLogFile(ex);
                // אם הוא לא מצליח לשמור 
            }

            _succes = _response.Length == 0;// CheckErrors(objRes);
            try
            {
                //שמירה של מסמך התשובה
                if (!_succes && !string.IsNullOrEmpty(savePath))
                    objRes.save(savePath + "_ERROR.xml");
                else
                    objRes.save(savePath + suffix);


            }
            catch (Exception ex)
            {

                Logger.WriteLogFile(ex);
                // אם הוא לא מצליח לשמור 
            }

            return _succes;
        }

        public bool ProcssXmlWithOutResponse()
        {
            objRes = new DOMDocument();
            //_response = _processXml.ProcessXMLWithResponse(objDoc, objRes);
            _response = _processXml.ProcessXML(objDoc);

            try
            {
                var directoryPath = GetDirectoryPath();
                if (directoryPath != null)
                {
                    //     for test
                    //    objDoc.save(@"C:\temp\" + "Doc" + (DateTime.Now.ToString().MakeSafeFilename('_')) + ".xml");
                    //    objRes.save(@"C:\temp\" + "Res" + (DateTime.Now.ToString().MakeSafeFilename('_')) + ".xml");

                    string ut = DateTime.UtcNow.ToString("yyyy-MM-dd HH-mm-ss-fff");


                //    objDoc.save(directoryPath + "Doc_" + fileName.MakeSafeFilename('_') + (ut) + ".xml");
                 //   objRes.save(directoryPath + "Res_" + fileName.MakeSafeFilename('_') + (ut) + ".xml");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLogFile(ex);
                // אם הוא לא מצליח לשמור 
            }

            _succes = _response.Length == 0;// CheckErrors(objRes);

            return _succes;
        }

        public bool ProcssXmlWithOutSave()
        {
            objRes = new DOMDocument();
            _response = _processXml.ProcessXMLWithResponse(objDoc, objRes);

            _succes = _response.Length == 0;// CheckErrors(objRes);

            return _succes;
        }
        /// <summary>
        /// Get directory to save docs
        /// </summary>
        /// <returns></returns>
        private string GetDirectoryPath()
        {



            if (!string.IsNullOrEmpty(path))
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }


            return path;
        }

        private bool CheckErrors(DOMDocument objRes)
        {
            try
            {
                IXMLDOMNode answer = objRes.getElementsByTagName("errors")[0];
                return answer == null;
            }
            catch (Exception ex)
            {
                Logger.WriteLogFile(ex);
            }
            return true;
        }

        /// <summary>
        /// Get response from xml by tag
        /// </summary>
        /// <param name="tagName">tag with response data</param>
        /// <returns></returns>
        public string GetValueByTagName(string tagName)
        {
            try
            {

                if (objRes != null)
                {
                    var newValue = ((dynamic)objRes.getElementsByTagName(tagName)[0]).nodeTypedValue;
                    return newValue != null ? newValue.ToString() : null;
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLogFile(ex);

            }
            return null;

        }

        /// <summary>
        /// Get response from xml by tag
        /// </summary>
        /// <param name="tagName">tag with response data</param>
        /// <param name="index">Location of tag in response</param>
        /// <returns></returns>
        public object GetValueByTagName(string tagName, int index)
        {
            try
            {

                if (objRes != null)
                {
                    var newValue = ((dynamic)objRes.getElementsByTagName(tagName)[index]).nodeTypedValue;
                    return newValue != null ? newValue.ToString() : null;
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLogFile(ex);

            }
            return null;
        }
        /// <summary>
        /// Get response from xml by tag
        /// </summary>
        /// <param name="tagName">tag with response data</param>
        /// <param name="index">Location of tag in response</param>
        /// <returns></returns>
        public object GetValueByTagName(string tagName, int index, int attributeIndex)
        {
            try
            {

                if (objRes != null)
                {
                    var newValue = ((dynamic)((objRes.getElementsByTagName(tagName)[index]))).attributes[attributeIndex].text;
                    return newValue != null ? newValue.ToString() : null;
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLogFile(ex);

            }
            return null;
        }
        /// <summary>
        /// Get string error
        /// </summary>
        /// <returns></returns>
        public string ErrorResponse
        {
            get
            {
                if (!_succes)
                {
                    return _response;
                }
                return null;
            }
        }
    }
}

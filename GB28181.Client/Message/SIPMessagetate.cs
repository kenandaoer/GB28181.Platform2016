﻿// ============================================================================
// FileName: SIPRegistrarState.cs
//
// Description:
// Application configuration for a SIP Registrar Server.
//
// Author(s):
// Aaron Clauson
//
// History:
// 29 Mar 2009	Aaron Clauson	Created.
//
// License: 
// This software is licensed under the BSD License http://www.opensource.org/licenses/bsd-license.php
//
// Copyright (c) 2009 Aaron Clauson (aaronc@blueface.ie), Blue Face Ltd, Dublin, Ireland (www.blueface.ie)
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that 
// the following conditions are met:
//
// Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer. 
// Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following 
// disclaimer in the documentation and/or other materials provided with the distribution. Neither the name of Blue Face Ltd. 
// nor the names of its contributors may be used to endorse or promote products derived from this software without specific 
// prior written permission. 
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, 
// BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
// IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, 
// OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, 
// OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, 
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE.
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using Logger4Net;
using SIPSorcery.GB28181.Sys;
using System.Configuration;

namespace Gb28181_Client.Message
{
    /// <summary>
    /// Retrieves application conifguration settings from App.Config.
    /// </summary>
    public class SIPMessageState 
    {
        private const string LOGGER_NAME = "siprergistrar";

        public const string SIPREGISTRAR_CONFIGNODE_NAME = "sipServer";

        private const string SIPSOCKETS_CONFIGNODE_NAME = "sipsockets";
        private const string USERAGENTS_CONFIGNODE_NAME = "useragentconfigs";
        private const string MONITOR_LOOPBACK_PORT_KEY = "MonitorLoopbackPort";
        private const string MAXIMUM_ACCOUNT_BINDINGS_KEY = "MaximumAccountBindings";
        private const string NATKEEPALIVE_RELAY_SOCKET = "NATKeepAliveRelaySocket";
        //private const string SWITCHBOARD_CERTIFICATE_NAME_KEY = "SwitchboardCertificateName";
        private const string SWITCHBOARD_USERAGNET_PREFIX_KEY = "SwitchboardUserAgentPrefix";
        private const string THREAD_COUNT_KEY = "ThreadCount";

        public static ILog logger;

        private static readonly XmlNode m_sipRegistrarNode;
        public static readonly XmlNode SIPRegistrarSocketsNode;
        public static readonly XmlNode UserAgentsConfigNode;

        static SIPMessageState()
        {
            try
            {
                #region Configure logging.

                try
                {
                  //  Logger4Net.Config.XmlConfigurator.Configure();
                    logger = Logger4Net.LogManager.GetLogger(LOGGER_NAME);
                }
                catch (Exception logExcp)
                {
                    Console.WriteLine("Exception SIPMessageState Configure Logging. " + logExcp.Message);
                }

                #endregion

                //if (AppState.GetSection(SIPREGISTRAR_CONFIGNODE_NAME) != null)
                //{
                //    m_sipRegistrarNode = (XmlNode)AppState.GetSection(SIPREGISTRAR_CONFIGNODE_NAME);
                //}

                XmlDocument doc = new XmlDocument();
                string xml = AppDomain.CurrentDomain.BaseDirectory + "Config\\SipSocket.xml";
                doc.Load(xml);
                m_sipRegistrarNode = doc.SelectNodes("sipServer")[0];

                if (m_sipRegistrarNode == null)
                {
                    //throw new ApplicationException("The SIP Registrar could not be started, no " + SIPREGISTRAR_CONFIGNODE_NAME + " config node available.");
                    logger.Warn("The SIP Registrar " + SIPREGISTRAR_CONFIGNODE_NAME + " config node was not available, the agent will not be able to start.");
                }
                else
                {
                    SIPRegistrarSocketsNode = m_sipRegistrarNode.SelectSingleNode(SIPSOCKETS_CONFIGNODE_NAME);
                    if (SIPRegistrarSocketsNode == null)
                    {
                        throw new ApplicationException("The SIP Registrar could not be started, no " + SIPSOCKETS_CONFIGNODE_NAME + " node could be found.");
                    }

                    UserAgentsConfigNode = m_sipRegistrarNode.SelectSingleNode(USERAGENTS_CONFIGNODE_NAME);
                }
            }
            catch (Exception excp)
            {
                logger.Error("Exception SIPRegistrarState. " + excp.Message);
                throw;
            }
        }
    }
}

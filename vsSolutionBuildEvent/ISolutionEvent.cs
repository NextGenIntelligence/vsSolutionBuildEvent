﻿/* 
 * Boost Software License - Version 1.0 - August 17th, 2003
 * 
 * Copyright (c) 2013 Developed by reg <entry.reg@gmail.com>
 * 
 * Permission is hereby granted, free of charge, to any person or organization
 * obtaining a copy of the software and accompanying documentation covered by
 * this license (the "Software") to use, reproduce, display, distribute,
 * execute, and transmit the Software, and to prepare derivative works of the
 * Software, and to permit third-parties to whom the Software is furnished to
 * do so, all subject to the following:
 * 
 * The copyright notices in the Software and this entire statement, including
 * the above license grant, this restriction and the following disclaimer,
 * must be included in all copies of the Software, in whole or in part, and
 * all derivative works of the Software, unless such copies or derivative
 * works are solely in the form of machine-executable object code generated by
 * a source language processor.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE, TITLE AND NON-INFRINGEMENT. IN NO EVENT
 * SHALL THE COPYRIGHT HOLDERS OR ANYONE DISTRIBUTING THE SOFTWARE BE LIABLE
 * FOR ANY DAMAGES OR OTHER LIABILITY, WHETHER IN CONTRACT, TORT OR OTHERWISE,
 * ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
 * DEALINGS IN THE SOFTWARE. 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace reg.ext.vsSolutionBuildEvent
{
    interface ISolutionEvent
    {
        /// <summary>
        /// execution of shell command
        /// </summary>
        string command { get; set; }

        /// <summary>
        /// output information to "Output" window or something else...
        /// </summary>
        string caption { get; set; }

        /// <summary>
        /// status of activate
        /// </summary>
        bool enabled { get; set; }

        /// <summary>
        /// Hide Process
        /// </summary>
        bool processHide { get; set; }

        /// <summary>
        /// not close after completion
        /// </summary>
        bool processKeep { get; set; }

        /// <summary>
        /// script or files mode
        /// </summary>
        bool modeScript { get; set; }

        /// <summary>
        /// stream processor
        /// </summary>
        string interpreter { get; set; }

        /// <summary>
        /// treat newline as
        /// </summary>
        string newline { get; set; }

        /// <summary>
        /// symbol wrapper for commands or script
        /// </summary>
        string wrapper { get; set; }

        /// <summary>
        /// Wait until terminates script handling
        /// </summary>
        bool waitForExit { get; set; }

        /// <summary>
        /// support of MSBuild environment variables (properties)
        /// </summary>
        bool parseVariablesMSBuild { get; set; }
    }
}
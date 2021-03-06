﻿/*
 * Copyright (c) 2013-2015  Denis Kuzmin (reg) <entry.reg@gmail.com>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using net.r_eg.vsSBE.MSBuild;
using net.r_eg.vsSBE.Scripts;

namespace net.r_eg.vsSBE.SBEScripts.Components
{
    public abstract class Component: IComponent
    {
        /// <summary>
        /// Ability to work with data for current component
        /// </summary>
        public abstract string Condition { get; }

        /// <summary>
        /// Handler for current data
        /// </summary>
        /// <param name="data">mixed data</param>
        /// <returns>prepared and evaluated data</returns>
        public abstract string parse(string data);

        /// <summary>
        /// Allows post-processing with MSBuild core.
        /// In general, some components can require immediate processing with evaluation, before passing control to next level
        /// </summary>
        public bool PostProcessingMSBuild
        {
            get { return postProcessingMSBuild; }
            set { postProcessingMSBuild = value; }
        }
        protected bool postProcessingMSBuild = false;

        /// <summary>
        /// Activation status
        /// </summary>
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }
        protected bool enabled = true;

        /// <summary>
        /// Sets location "as is" - after deepening
        /// </summary>
        public bool BeforeDeepen
        {
            get { return beforeDeepen; }
        }
        protected bool beforeDeepen = false;

        /// <summary>
        /// Disabled the forced post analysis
        /// </summary>
        public bool PostParse
        {
            get { return postParse; }
        }
        protected bool postParse = false;

        /// <summary>
        /// Disabled regex engine for property - condition
        /// </summary>
        public bool CRegex
        {
            get { return cregex; }
        }
        protected bool cregex = false;

        /// <summary>
        /// For evaluating with SBE-Script
        /// </summary>
        protected ISBEScript script;

        /// <summary>
        /// For evaluating with MSBuild
        /// </summary>
        protected IMSBuild msbuild;

        /// <summary>
        /// Provides operation with environment
        /// </summary>
        protected IEnvironment env;

        /// <summary>
        /// Current container of user-variables
        /// </summary>
        protected IUserVariable uvariable;

        /// <param name="env">Used environment</param>
        /// <param name="uvariable">Used instance of user-variables</param>
        public Component(IEnvironment env, IUserVariable uvariable): this()
        {
            this.env        = env;
            this.uvariable  = uvariable;
            script          = new Script(env, uvariable);
            msbuild         = new MSBuild.Parser(env, uvariable);
        }

        /// <param name="env">Used environment</param>
        public Component(IEnvironment env): this()
        {
            this.env = env;
        }

        public Component()
        {
            Log.nlog.Trace("init: '{0}'", this.ToString());
        }
    }
}

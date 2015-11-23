using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Dashboard.Models.Net
{
    public class Path
    {
        private String basePath;
        public String BasePath
        {
            get
            {
                return this.basePath;
            }
            set
            {
                this.basePath = value;
                this.dirty = true;
            }
        }

        private Dictionary<String, String> parameters;
        private String cachedString;
        private Boolean dirty;

        public Path(String basePath)
        {
            BasePath = basePath;
            this.parameters = new Dictionary<string, string>();
            this.dirty = true;
        }

        public void AddParameter(String key, String value)
        {
            this.parameters.Add(key, value);
            this.dirty = true;
        }

        public String ToString()
        {
            if(this.dirty)
            {
                this.dirty = false;

                StringBuilder builder = new StringBuilder(256);
                builder.Append(BasePath);

                int index = 0;
                foreach(var item in this.parameters)
                {
                    builder.Append(index == 0 ? '?' : '&');
                    builder.Append(item.Key);
                    builder.Append('=');
                    builder.Append(item.Value);

                    index++;
                }

                this.cachedString = builder.ToString();
            }

            return this.cachedString;
        }
    }
}
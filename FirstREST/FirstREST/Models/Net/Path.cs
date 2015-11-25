using System;
using System.Collections.Generic;
using System.Text;

namespace Dashboard.Models.Net
{
    public class Path
    {
        private String _basePath;
        public String BasePath
        {
            get
            {
                return _basePath;
            }
            set
            {
                _basePath = value;
                _dirty = true;
            }
        }

        private Dictionary<String, String> _parameters;
        private String _cachedString;
        private Boolean _dirty;

        public Path(String basePath)
        {
            BasePath = basePath;
            _parameters = new Dictionary<string, string>();
            _dirty = true;
        }

        public void AddParameter(String key, String value)
        {
            _parameters.Add(key, value);
            _dirty = true;
        }

        public override String ToString()
        {
            if(_dirty)
            {
                _dirty = false;

                StringBuilder builder = new StringBuilder(256);
                builder.Append(BasePath);

                int index = 0;
                foreach(var item in _parameters)
                {
                    builder.Append(index == 0 ? '?' : '&');
                    builder.Append(item.Key);
                    builder.Append('=');
                    builder.Append(item.Value);

                    index++;
                }

                _cachedString = builder.ToString();
            }

            return _cachedString;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MMXModelsLib {
    public abstract class BaseModel  {
        private bool isAdminRole = false;
        public bool IsAdminRole {
            get { return isAdminRole; }
            set { isAdminRole = value; }
        }

        private List<ErrorMessageModel> errors;
        public List<ErrorMessageModel> Errors {
            get {
                if (errors == null) {
                    errors = new List<ErrorMessageModel>();
                }
                return errors;
            }
            set { errors = value; }
        }

    }
}

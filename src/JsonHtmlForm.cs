using System;
using System.Collections.Generic;
using System.Reflection;
using ServiceStack.Text;

namespace JsonHtml
{
    public interface IJsonHtmlForm
    {
        dynamic GenerateDynamicForm(Type type, string desc);
    }

    public class JsonHtmlForm : IJsonHtmlForm
    {
        protected Dictionary<string, Func<dynamic>> nameElementAction = new Dictionary<string, Func<dynamic>>();
        protected Dictionary<Type, Func<dynamic>> typeElementAction = new Dictionary<Type, Func<dynamic>>();
        protected String caption;
        protected String id;
        protected String name;

        public JsonHtmlForm()
        {
            nameElementAction.Add("email", () => EmailField());
            nameElementAction.Add("description", () => TextArea());
            nameElementAction.Add("password", () => PasswordField());
            nameElementAction.Add("confirm_password", () => PasswordRepeatField());
            nameElementAction.Add("gender", () => GenderField());
            nameElementAction.Add("state", () => StateAutoComplete());

            typeElementAction.Add(typeof (String), () => TextBox());
            typeElementAction.Add(typeof (Int32), () => TextBox());
            typeElementAction.Add(typeof (Decimal), () => TextBox());
            typeElementAction.Add(typeof (DateTime), () => DatePicker());
            typeElementAction.Add(typeof (Boolean), () => BooleanCheckbox());
        }


/*
        {
    "action" : "index.html",
    "method" : "post",
    "elements" :
    [
        {
            "type" : "fieldset",
            "caption" : "User information",
            "elements" :
            [
                {
                    "name" : "email",
                    "caption" : "Email address",
                    "type" : "text",
                    "placeholder" : "E.g. user@example.com",
                    "validate" :
                    {
                    "email" : true
                    }
                },
                {
                    "name" : "password",
                    "caption" : "Password",
                    "type" : "password",
                    "id" : "registration-password",
                    "validate" :
                    {
                        "required" : true,
                        "minlength" : 5,
                        "messages" :
                        {
                            "required" : "Please enter a password",
                            "minlength" : "At least {0} characters long"
                        }
                    }
                },
                {
                    "name" : "password-repeat",
                    "caption" : "Repeat password",
                    "type" : "password",
                    "validate" :
                    {
                        "equalTo" : "#registration-password",
                        "messages" :
                        {
                            "equalTo" : "Please repeat your password"
                        }
                    }
                },
                {
                    "type" : "radiobuttons",
                    "caption" : "Sex",
                    "name" : "sex",
                    "class" : "labellist",
                    "options" :
                    {
                        "f" : "Female",
                        "m" : "Male"
                    }
                },
                {
                    "type" : "checkboxes",
                    "name" : "test",
                    "caption" : "Receive newsletter about",
                    "class" : "labellist",
                    "options" :
                    {
                        "updates" : "Product updates",
                        "errors" :
                        {
                        "value" : "security",
                        "caption" : "Security warnings",
                        "checked" : "checked"
                        }
                    }
                },
                  {
                    "type" : "select",
                    "name" : "continent",
                    "caption" : "Choose a continent",
                    "options" :
                    {
                        "america" : "America",
                        "europe" :
                        {
                            "selected" : "true",
                            "id" : "europe-option",
                            "value" : "europe",
                            "html" : "Europe"
                        },
                        "asia" : "Asia",
                        "africa" : "Africa",
                        "australia" : "Australia"
                    }
                    ,
        {
            "name" : "textfield",
            "caption" : "Autocomplete",
            "type" : "text",
            "placeholder" : "Type 'A' or 'S'",
            "autocomplete" :
            {
                "source" :  [ "Apple", "Acer", "Sony", "Summer" ]
            }
        },
        {
            "name" : "date",
            "caption" : "Datepicker",
            "type" : "text",
            "datepicker" : {  "showOn" : "button" }
        },
                }
            ]
        },
      
        {
            "type" : "submit",  
            "value" : "Signup"
        }
    ]
}*/

        protected dynamic EmailField()
        {
            dynamic email = new
                                {
                                    name,
                                    id,
                                    caption,
                                    type = "text",
                                    placeholder = "E.g. user@example.com",
                                    validate = new {email = true}
                                };
            return email;
        }

        protected dynamic PasswordField()
        {
            dynamic password = new
                                   {
                                       name,
                                       id,
                                       caption,
                                       type = "password",
                                       validate = new
                                                      {
                                                          required = true,
                                                          minlength = 5,
                                                          message = new
                                                                        {
                                                                            required = "Please enter a password",
                                                                            minlength = "At least {0} characters long"
                                                                        }
                                                      }
                                   };


            return password;
        }


        protected dynamic PasswordRepeatField()
        {
            dynamic password = new
                                   {
                                       name,
                                       id,
                                       caption,
                                       type = "password",
                                       validate = new
                                                      {
                                                          required = true,
                                                          minlength = 5,
                                                          message = new
                                                                        {
                                                                            equalTo = "#password"
                                                                        }
                                                      }
                                   };


            return password;
        }

        protected dynamic StateAutoComplete()
        {
//            {
//            "name" : "textfield",
//            "caption" : "Autocomplete",
//            "type" : "text",
//            "placeholder" : "Type 'A' or 'S'",
//            "autocomplete" :
//            {
//                "source" :  [ "Apple", "Acer", "Sony", "Summer" ]
//            }
//        }
            dynamic stateAutoComplete = new
                                            {
                                                name,
                                                id,
                                                caption,
                                                type = "text",
                                                placeholder = "Type first letter of State",
                                                autocomplete = new
                                                                   {
                                                                       list = new dynamic[]
                                                                                  {
                                                                                      "AK", "AL", "AR", "AZ", "CA", "CO"
                                                                                      , "CT", "DC", "DE", "FL", "GA",
                                                                                      "HI", "IA", "ID",
                                                                                      "IL", "IN", "KS", "KY", "LA", "MA"
                                                                                      , "MD", "ME", "MI", "MN", "MO",
                                                                                      "MS", "MT", "NC",
                                                                                      "ND", "NE", "NH", "NJ", "NM", "NV"
                                                                                      , "NY", "OH", "OK", "OR", "PA",
                                                                                      "RI", "SC", "SD",
                                                                                      "TN", "TX", "UT", "VA", "VT", "WA"
                                                                                      , "WI", "WV", "WY"
                                                                                  }
                                                                   }
                                            };


            return stateAutoComplete;
        }


        protected dynamic GenderField()
        {
//         {
//                    "type" : "radiobuttons",
//                    "caption" : "Sex",
//                    "name" : "sex",
//                    "class" : "labellist",
//                    "options" :
//                    {
//                        "f" : "Female",
//                        "m" : "Male"
//                    }
//                },

            dynamic gender = new
                                 {
                                     name,
                                     id,
                                     caption,
                                     type = "radiobuttons",
                                     options = new
                                                   {
                                                       f = "Female",
                                                       m = "Male"
                                                   }
                                 };

            return gender;
        }

        protected dynamic DatePicker()
        {
            dynamic date = new
                               {
                                   name,
                                   id,
                                   caption,
                                   type = "text",
                                   datepicker = new
                                                    {
                                                        showOn = "button"
                                                    }
                               };


            return date;
        }

        protected dynamic TextArea()
        {
            dynamic textbox = new
                                  {
                                      type = "textarea",
                                      cols = 50,
                                      rows = 10,
                                      name,
                                      id,
                                      caption,
                                      validate = new
                                                     {
                                                         required = true
                                                     }
                                  };

            return textbox;
        }


        protected dynamic TextBox()
        {
            dynamic textbox = new
                                  {
                                      type = "text",
                                      name,
                                      id,
                                      caption,
                                      validate = new
                                                     {
                                                         required = true
                                                     }
                                  };

            return textbox;
        }

        protected dynamic Select(List<string> enumList)
        {
            dynamic select = new 
//             select.Add("type", "select");
//               select.Add("name", name);
//               select.Add("id",  id);
//               select.Add("caption", caption);
//             select.Add("validate", new);
//               select.Add("type", "select");

                                 {
                                     type = "select",
                                     name,
                                     id,
                                     caption,
                                     validate = new
                                                    {
                                                        required = true
                                                    },
                                     options = new JsonObject()
                                 };

            foreach (string enumVal in enumList)
            {
                select.options.Add(enumVal, enumVal);
            }
            return select;
        }


        protected dynamic BooleanCheckbox()
        {
//             {
//                    "type" : "checkboxes",
//                    "name" : "test",
//                    "caption" : "Receive newsletter about",
//                    "class" : "labellist",
//                    "options" :
//                    {
//                        "updates" : "Product updates",
//                        "errors" :
//                        {
//                        "value" : "security",
//                        "caption" : "Security warnings",
//                        "checked" : "checked"
//                        }
//                    }
//                },

            dynamic boolCheckbox = new
                                       {
                                           name,
                                           id,
                                           caption,
                                           type = "checkbox"
                                       };

            return boolCheckbox;
        }

        public dynamic GenerateDynamicForm(Type type, string desc)
        {

            
            PropertyInfo[] propertyInfos = type.GetProperties();

            var fieldsetElements = new List<dynamic>();
            dynamic element;

            foreach (PropertyInfo pi in propertyInfos)
            {
                element = GetElement(pi);


                if (element != null)
                {
                    fieldsetElements.Add(new {type = "container", Class = "clear"});
                    fieldsetElements.Add(new {type = "container", Class = "three columns"});
                    fieldsetElements.Add(element);
                    fieldsetElements.Add(new {type = "container", Class = "five columns"});
                }
            }
            var fieldset =
                new
                    {
                        elements = fieldsetElements,
                        type = "fieldset",
                        caption = desc,
                        Class = "sixteen columns",
                        Id = "form-fieldset"
                    };
            var submitButton = new {type = "submit", value = desc};
            var formElements = new List<dynamic>();
            formElements.Add(fieldset);
            formElements.Add(submitButton);
            return new {elements = formElements};
        }

        protected dynamic GetElement(PropertyInfo pi)
        {
            id = pi.Name;
            name = pi.Name;
            caption = pi.Name.Replace("_", " ");

            if (pi.PropertyType.IsEnum)
            {
                var enumList = new List<string>();
                foreach (FieldInfo fInfo in pi.PropertyType.GetFields(BindingFlags.Public | BindingFlags.Static))
                {
                    enumList.Add(fInfo.GetValue(null).ToString());
                }

                var e = new List<dynamic>();
                e.Add(Select(enumList));
                return new {type = "container", elements = e, Class = "eight columns elementWrapper"};
            }

            if (nameElementAction.ContainsKey(pi.Name.ToLower()))
            {
                var e = new List<dynamic>();
                e.Add(nameElementAction[pi.Name.ToLower()].Invoke());
                return new {type = "container", elements = e, Class = "eight columns elementWrapper"};
            }

            if (typeElementAction.ContainsKey(pi.PropertyType))
            {
                var e = new List<dynamic>();
                e.Add(typeElementAction[pi.PropertyType].Invoke());
                return new {type = "container", elements = e, Class = "eight columns elementWrapper"};
            }

            else return null;
        }

        public void GenerateDynamicForm<T>(T entity)
        {
            PropertyInfo[] propertyInfos = entity.GetType().GetProperties();

            var elements = new List<dynamic>();
            dynamic element;

            foreach (PropertyInfo pi in propertyInfos)
            {
                Type type = pi.PropertyType;
                object value = pi.GetValue(entity, null);
                string name = pi.Name;
                element = GetElement(pi);
                if (element != null)
                {
                    element.id = name;
                    element.name = name;
                    element.caption = name;
                    element.value = value;

                    elements.Add(element);
                }
            }
        }
    }
}
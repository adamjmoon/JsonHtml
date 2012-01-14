using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Reflection;
using System.Web.UI.WebControls;
using JsonHtmlTable.Strategies;
using ServiceStack.Text;

namespace JsonHtmlTable
{
    public class JsonHtmlForm
    {

        private String id;
        private String name;
        private String caption;

         private Dictionary<string, Func<dynamic>> nameElementAction = new Dictionary<string, Func<dynamic>>();
         private Dictionary<Type, Func<dynamic>> typeElementAction = new Dictionary<Type, Func<dynamic>>();

         public JsonHtmlForm()
         {
             nameElementAction.Add("email", () => EmailField());
             nameElementAction.Add("password", () => PasswordField());
             nameElementAction.Add("passwordRepeat", () => PasswordRepeatField());
             nameElementAction.Add("gender", () => GenderField());
             nameElementAction.Add("state", () => StateAutoComplete());

             typeElementAction.Add(typeof(String), () => TextBox());
             typeElementAction.Add(typeof(Int32), () => TextBox());
             typeElementAction.Add(typeof(Decimal), () => TextBox());
             typeElementAction.Add(typeof(DateTime), () => DatePicker());
             typeElementAction.Add(typeof(Boolean), () => BooleanCheckbox());
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

        private dynamic EmailField()
        {
            dynamic email = new
                                {
                                    name = name,
                                    id = id,
                                    caption = caption,
                                    type = "text",
                                    placeholder = "E.g. user@example.com",
                                    validate = new { email = true}
                                };
            return email;

        }

        private dynamic PasswordField()
        {
            dynamic password = new
                                   {
                                       name = "",
                                       id = "",
                                       caption = "",
                                       type = "password",
                                       validate = new { required = true,
                                                        minlength = 5,
                                                        message = new {
                                                             required = "Please enter a password",
                                                             minlength = "At least {0} characters long"
                                                          }
                                                      }
                                   };
            

            return password;

        }


        private dynamic PasswordRepeatField()
        {
            dynamic password = new
                                   {
                                       name = name,
                                       id = id,
                                       caption = caption,
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

        private dynamic StateAutoComplete()
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
                                                name = name,
                                                id = id,
                                                caption = caption,
                                                type = "text",
                                                placeholder = "Type first letter of State",
                                                autocomplete = new 
                                                                   { 
                                                                        list =  new dynamic[]
                                                                         {
                                                                             "AK", "AL", "AR", "AZ", "CA", "CO", "CT", "DC", "DE", "FL", "GA", "HI", "IA", "ID",
                                                                             "IL", "IN", "KS", "KY", "LA", "MA", "MD", "ME", "MI", "MN", "MO", "MS", "MT", "NC",
                                                                             "ND", "NE", "NH", "NJ", "NM", "NV", "NY", "OH", "OK", "OR", "PA", "RI", "SC", "SD",
                                                                             "TN", "TX", "UT", "VA", "VT", "WA", "WI", "WV", "WY"
                                                                         }
                                                                   }

                                            };
           

            return stateAutoComplete;
        }

        

        private dynamic GenderField()
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
                                     name = name,
                                     id = id,
                                     caption = caption,
                                     type = "radiobuttons",
                                     options = new {
                                       f = "Female",
                                       m = "Male"
                                     }
                                 };

            return gender;
        }

        private dynamic DatePicker()
        {
            dynamic date = new
                               {
                                   name = name,
                                   id = id,
                                   caption = caption,
                                   type = "text",
                                   datepicker = new
                                                    {
                                                        showOn = "button" 
                                                    }
                               };
           
          
            return date;
        }

         private dynamic TextBox()
        {
            dynamic textbox = new
                                  {
                                      type = "text",
                                      name = name,
                                      id = id,
                                      caption = caption,
                                      validate = new
                                      {
                                          required = true
                                      }
                                  };

            return textbox;

        }

         private dynamic Select(List<string> enumList)
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
                 name = name,
                 id = id,
                 caption = caption,
                 validate = new
                 {
                     required = true
                 },
                 options = new JsonObject()
             };

             foreach(var enumVal in enumList)
             {
                 select.options.Add(enumVal, enumVal);
             }
             return select;
         }


        private dynamic BooleanCheckbox()
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
                                           name = name,
                                           id = id,
                                           caption = caption,
                                           type = "checkbox"
                                       };
          
            return boolCheckbox;
        }
       
        public dynamic GenerateDynamicForm(Type type, string desc)
        {

            PropertyInfo[] propertyInfos = type.GetProperties();



            var fieldsetElements = new List<dynamic>();
            dynamic element;

            foreach (var pi in propertyInfos)
            {
              
                element = GetElement(pi);
                
                
                if(element != null)
                {
                    fieldsetElements.Add(new { type = "container", Class = "clear" });
                    fieldsetElements.Add(new { type = "container", Class = "four columns" });
                    fieldsetElements.Add(element);
                    fieldsetElements.Add(new { type = "container", Class = "four columns" });
                    
//                    if (name.Equals("password"))
//                    {
//                        fieldsetElements.Add(new { type = "container", Class = "clear" }); 
//                        fieldsetElements.Add(new { type = "container", Class = "four columns" });
//                        fieldsetElements.Add(GetElement("password_Repeat", null));
//                        fieldsetElements.Add(new { type = "container", Class = "four columns" });
//                        
//                    }
                }
            }
            var fieldset = new { elements = fieldsetElements, type = "fieldset", caption = desc, Class="sixteen columns"};
            var submitButton = new {type = "submit", value = desc };
            var formElements = new List<dynamic>();
            formElements.Add(fieldset);
            formElements.Add(submitButton);
            return new { elements = formElements };
        }

        private dynamic GetElement(PropertyInfo pi)
        {
            this.id = pi.Name;
            this.name = pi.Name;
            this.caption = pi.Name.Replace("_"," ");

            if (pi.PropertyType.IsEnum)
            {
                var enumList = new List<string>();
                foreach (FieldInfo fInfo in pi.PropertyType.GetFields(BindingFlags.Public | BindingFlags.Static))
                {
                   enumList.Add(fInfo.GetValue(null).ToString());
                   
                }

                var e = new List<dynamic>();
                e.Add(Select(enumList));
                return new { type = "container", elements = e, Class = "eight columns elementWrapper" };
            }

            if (nameElementAction.ContainsKey(pi.Name.ToLower()))
            {
                var e = new List<dynamic>();
                e.Add(nameElementAction[pi.Name.ToLower()].Invoke());
                return new { type = "container", elements = e, Class = "eight columns elementWrapper" };
            }

            if (typeElementAction.ContainsKey(pi.PropertyType))
            {
                var e = new List<dynamic>();
                e.Add(typeElementAction[pi.PropertyType].Invoke());
                return new { type = "container", elements = e, Class = "eight columns elementWrapper" };
            }

            else return null;
        }

        public void GenerateDynamicForm<T>(T entity)
        {
            PropertyInfo[] propertyInfos = entity.GetType().GetProperties();

            var elements = new List<dynamic>();
            dynamic element;

            foreach (var pi in propertyInfos)
            {
                var type = pi.PropertyType;
                var value = pi.GetValue(entity, null);
                var name = pi.Name;
                element = GetElement(pi);
                if (element != null)
                {
                    element.id = name;
                    element.name = name;
                    element.caption = name;
                    element.value = value;

                    elements.Add(element);

//                    if (name.Equals("password"))
//                    {
//                        elements.Add(GetElement("passwordRepeat", null));
//                    }
                }
            }
        }


    }
    
}
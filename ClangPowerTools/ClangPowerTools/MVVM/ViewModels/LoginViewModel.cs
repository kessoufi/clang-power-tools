﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ClangPowerTools
{
  public class LoginViewModel : INotifyPropertyChanged, IDataErrorInfo
  {
    #region Members

    private UserModel userModel = new UserModel();
    public event PropertyChangedEventHandler PropertyChanged;

    #endregion

    #region Properties

    public string Email
    {
      get { return userModel.email; }
      set
      {
        userModel.email = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Email"));
      }
    }

    public string Password
    {
      get { return userModel.password; }
      set
      {
        userModel.password = value;
      }
    }

    public bool InvalidInput { get; set; } = false;

    #endregion

    #region IDataErrorInfo Interface

    public string Error => null;

    public string this[string name]
    {
      get
      {
        string result = null;

        switch(name)
        {
          case "Email":
            InvalidInput = IsEmailAddressValid(out string errorMessage);
            result = errorMessage;

            break;
        }
        return result;
      }
    }

    #endregion

    #region Private Methods

    private bool IsEmailAddressValid(out string errorMessage )
    {
      errorMessage = null;
      var validEmailAddress = new EmailAddressAttribute().IsValid(Email);

      if (validEmailAddress == false)
      {
        errorMessage = "Email address is required";
        return false;
      }

      return true;
    }

    #endregion



  }
}

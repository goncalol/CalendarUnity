using UnityEngine;
using TMPro;
using System.Linq;
using System;
using Assets.Scripts.Messages;
using System.Globalization;
using System.Collections.Generic;

public class PopupForm : MonoBehaviour
{
    public TMP_InputField NameInput;
    public TMP_InputField StartDateInput;
    public TMP_InputField EndDateInput;
    public TMP_Dropdown CategoryInput;
    public TextMeshProUGUI Errors;

    public GameObject LoadingPanel;
    public GameObject DatePicker;

    public Calendar Calendar;

    private const string DateFormat = "dd/MM/yyyy HH:mm";
    private bool StartDateClicked;

    private void Awake()
    {
        StartDateInput.readOnly = true;
        EndDateInput.readOnly = true;
    }

    public void CreateTodo()
    {
        var name = NameInput.text;
        DateTime startDate;
        DateTime endDateTemp;
        DateTime? endDate = null;

        if (string.IsNullOrWhiteSpace(name))
        {
            Errors.SetText("- Name must not be empty");
            return;
        }
        if (!DateTime.TryParseExact(StartDateInput.text, DateFormat, CultureInfo.InvariantCulture,DateTimeStyles.None, out startDate))
        {
            Errors.SetText("- Start date not in format dd/MM/yyy HH:mm");
            return;
        }
        if (!string.IsNullOrWhiteSpace(EndDateInput.text))
        {
            if (DateTime.TryParseExact(EndDateInput.text, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out endDateTemp))
                endDate = endDateTemp;
            else
            {
                Errors.SetText("- End date not in format dd/MM/yyy HH:mm");
                return;
            }
            if (startDate >= endDate)
            {
                Errors.SetText("- End Date must be after Start Date");
                return;

            }
        }
        Errors.SetText(string.Empty);
        var category = CategoryInput.captionText.text;
        StartCoroutine(Calendar.CreateTodo(name, startDate.ToString(DateFormat), endDate?.ToString(DateFormat), category));
        LoadingPanel.SetActive(true);
    }

    public void SetCategories(Category[] categories)
    {
        CategoryInput.options= new List<TMP_Dropdown.OptionData>();
        CategoryInput.AddOptions(categories?.Select(e => e.name).ToList() ?? new string[] { string.Empty }.ToList());
    }

    internal void SetDate(DateTime date)
    {
        if(StartDateClicked)
            StartDateInput.text = date.ToString(DateFormat);
        else
            EndDateInput.text = date.ToString(DateFormat);
    }

    public void OnStartDateClicked()
    {
        StartDateClicked = true;
        DatePicker.SetActive(true);
        DatePicker.transform.GetChild(1).position = StartDateInput.transform.position - new Vector3(0,((RectTransform)StartDateInput.transform).rect.height);
    }

    public void OnEndDateClicked()
    {
        StartDateClicked = false;
        DatePicker.SetActive(true);
        DatePicker.transform.GetChild(1).position = EndDateInput.transform.position - new Vector3(0, ((RectTransform)EndDateInput.transform).rect.height);
    }
}

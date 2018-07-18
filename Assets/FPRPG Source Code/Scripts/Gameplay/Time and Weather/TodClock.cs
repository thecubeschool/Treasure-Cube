using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum WeekDays {
	Nedelni = 0,
	Ponedelni = 1,
	Dorogoi = 2,
	Sredni = 3,
	Cetvorti = 4,
	Pati = 5,
}

public enum Months {
	Hladnokozh = 0,
	Ledosek = 1,
	Derikoza = 2,
	Legotrav = 3,
	Cvetnik = 4,
	Treshnik = 5,
	Zhetnik = 6,
	Kosach = 7,
	Grozdober = 8,
	Drvopad = 9,
	Zimorod = 10,
	Praznikar = 11,
}

public class TodClock : MonoBehaviour {

	public string currentHour;
	public string currentMinute;
	public Text currentTime;

	public int daysPassedSinceStart;
	public int dayCount;
	public WeekDays currentDay;
	public Months currentMonth;
	public int currentYear;

	public Text dayMonthYearTxt;
	public Text dayMonthYearOnOvermapTxt;

	private bool countedDay;

	private TodWeather controller;
	public UISkillsAndAttributes uISkillsAndAttributes;
	
	void Awake() {
		controller = GetComponent<TodWeather>();
	}
	
	void Update() {
		float currentHourF = 24 * controller.currentTimeOfDay;
		float currentMinuteF = 60 * (currentHourF - Mathf.Floor(currentHourF));

		int currentHourI = (int)currentHourF;
		int currentMinuteI = (int)currentMinuteF;

		if(currentHourI < 10) {
			currentHour = "0"+currentHourI.ToString();
		}
		else {
			currentHour = currentHourI.ToString();
		}

		if(currentMinuteI < 10) {
			currentMinute = "0"+currentMinuteI.ToString();
		}
		else {
			currentMinute = currentMinuteI.ToString();
		}

		if(currentHourI == 6) {
			if(countedDay == false) {
				dayCount++;
				daysPassedSinceStart++;
				uISkillsAndAttributes.daysPassed.text = string.Empty;
				uISkillsAndAttributes.daysPassed.text = "Days passed: " + daysPassedSinceStart.ToString();
				UpdateDayMonthYear();
				countedDay = true;
			}
		}
		if(currentHourI == 18) {
			countedDay = false;
		}

		if(currentTime != null) {
			currentTime.text = currentHour + "h" + ":" + currentMinute + "m";
		}

		if(dayMonthYearTxt != null) {
			dayMonthYearTxt.text = dayCount + " " + currentMonth.ToString() + ", " + currentYear;
		}

		if(dayMonthYearOnOvermapTxt != null) {
			dayMonthYearOnOvermapTxt.text = dayCount + " " + currentMonth.ToString() + ", " + currentYear + ", " + currentHour + "h" + ":" + currentMinute + "m";
		}
	}

	void UpdateDayMonthYear() {

		if(currentDay == WeekDays.Nedelni) {
			currentDay = WeekDays.Ponedelni;
		}
		else if(currentDay == WeekDays.Ponedelni) {
			currentDay = WeekDays.Dorogoi;
		}
		else if(currentDay == WeekDays.Dorogoi) {
			currentDay = WeekDays.Sredni;
		}
		else if(currentDay == WeekDays.Sredni) {
			currentDay = WeekDays.Cetvorti;
		}
		else if(currentDay == WeekDays.Cetvorti) {
			currentDay = WeekDays.Pati;
		}
		else if(currentDay == WeekDays.Pati) {
			currentDay = WeekDays.Nedelni;
		}

		if(dayCount > 30) {

			if(currentMonth == Months.Hladnokozh) {
				currentMonth = Months.Ledosek;
			}
			else if(currentMonth == Months.Ledosek) {
				currentMonth = Months.Derikoza;
			}
			else if(currentMonth == Months.Derikoza) {
				currentMonth = Months.Legotrav;
			}
			else if(currentMonth == Months.Legotrav) {
				currentMonth = Months.Cvetnik;
			}
			else if(currentMonth == Months.Cvetnik) {
				currentMonth = Months.Treshnik;
			}
			else if(currentMonth == Months.Treshnik) {
				currentMonth = Months.Zhetnik;
			}
			else if(currentMonth == Months.Zhetnik) {
				currentMonth = Months.Kosach;
			}
			else if(currentMonth == Months.Kosach) {
				currentMonth = Months.Grozdober;
			}
			else if(currentMonth == Months.Grozdober) {
				currentMonth = Months.Drvopad;
			}
			else if(currentMonth == Months.Drvopad) {
				currentMonth = Months.Zimorod;
			}
			else if(currentMonth == Months.Zimorod) {
				currentMonth = Months.Praznikar;
			}
			else if(currentMonth == Months.Praznikar) {
				currentMonth = Months.Hladnokozh;
				currentYear++;
			}

			dayCount = 1;
		}
	}
}

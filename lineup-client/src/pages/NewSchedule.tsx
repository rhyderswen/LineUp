import type { DateDay, Time, TimeRange, ValidHours, ValidMinutes, Weekday } from "@/types";
//import { useAuth0 } from "@auth0/auth0-react";
import { useNavigate } from "react-router";
import React from "react";
import DatePickerModule, { DateObject } from "react-multi-date-picker";

// This is because importing DatePicker directly didn't work with Vite for some reason
// Trust me that this is somehow the most elegant solution I could find
// @ts-expect-error: react-multi-date-picker is funky with Vite
const DatePicker = DatePickerModule.default || DatePickerModule;

interface ScheduleData {
  name: string; //the name of the event
  shiftTimes: ValidMinutes | undefined; //how the availability intervals are determined
  dates: Date[] | undefined; //the dates being scheduled (js Date version)
  dateDays: DateDay[]; //the dates being schedules (DateDay version)
  hours: TimeRange; //the hours throughout the day that need covered
  pplPerShift: number | undefined; //how many people should work simultaneously

  // optional parameters
  maxShiftLength: number | undefined; //maximum length of time a single person can work continuously, in minutes
  maxShifts: number | undefined; //maximum number of shifts a single person can work
}

const NewSchedule = () => {
  const navigate = useNavigate();

  const [scheduleData, setScheduleData] = React.useState<ScheduleData>({
    name: "",
    shiftTimes: "" as ValidMinutes,
    dates: undefined,
    dateDays: [{ date: "1/1", day: "Thursday" }] as DateDay[],
    hours: { start: { hour: 9, minute: 0 }, end: { hour: 17, minute: 0 } } as TimeRange,
    pplPerShift: undefined,
    //optional parameters
    maxShiftLength: undefined,
    maxShifts: undefined,
  });

  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = event.target;

    setScheduleData((prev) => ({
      ...prev,
      [name]: event.target.type === "number" || name === "shiftTimes" ? Number(value) : value,
    }));
  };

  const handleSubmit = (event: React.SubmitEvent<HTMLFormElement>) => {
    event.preventDefault();

    console.log(
      "Submitted name:",
      scheduleData.name,
      "; Submitted avail. interval: ",
      scheduleData.shiftTimes,
      "; Submitted dates: ",
      scheduleData.dateDays,
      "; Submitted duration: ",
      scheduleData.hours.start,
      " to ",
      scheduleData.hours.end,
      "; Submitted pplpershift: ",
      scheduleData.pplPerShift,
      "; Submitted maximum shift time: ",
      scheduleData.maxShiftLength,
      "; Submitted maximum shifts: ",
      scheduleData.maxShifts,
    );

    //TODO: add to user's events
    //TODO: send data to backend
  };

  const handleStartTimeChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const newStart = parseTimeString(event.target.value);
    setScheduleData((prev) => ({
      ...prev,
      hours: {
        ...prev.hours,
        start: newStart,
      },
    }));
  };

  const handleEndTimeChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const newEnd = parseTimeString(event.target.value);
    setScheduleData((prev) => ({
      ...prev,
      hours: {
        ...prev.hours,
        end: newEnd,
      },
    }));
  };

  const parseTimeString = (time: string): Time => {
    const [hour, minute] = time.split(":").map(Number);

    return {
      hour: hour as ValidHours,
      minute: minute as ValidMinutes,
    };
  };

  const formatTimeString = (hour: number, minute: number) => {
    return `${hour.toString().padStart(2, "0")}:${minute.toString().padStart(2, "0")}`;
  };

  const checkForNull = () => {
    if (scheduleData.maxShiftLength == null) {
      setScheduleData((prev) => ({
        ...prev,
        maxShiftLength: 1440,
      }));
    }
    if (scheduleData.maxShifts == null) {
      setScheduleData((prev) => ({
        ...prev,
        maxShifts: 99999,
      }));
    }
  };

  const convertToDateDays = (dates: Date[]): DateDay[] => {
    return dates.map((jsDate) => ({
      date: `${jsDate.getMonth() + 1}/${jsDate.getDate()}`,
      day: jsDate.toLocaleDateString("en-US", {
        weekday: "long",
      }) as Weekday,
    }));
  };

  const handleDateChange = (value: DateObject | DateObject[] | null) => {
    if (!value) {
      setScheduleData((prev) => ({
        ...prev,
        dates: [],
        dateDays: [],
      }));
      return;
    }

    const dateArray = Array.isArray(value) ? value : [value];
    const jsDates = dateArray.map((d) => d.toDate());

    setScheduleData((prev) => ({
      ...prev,
      dates: jsDates,
      dateDays: convertToDateDays(jsDates),
    }));
  };

  return (
    <div className="newSchedule">
      <form onSubmit={handleSubmit}>
        <div>
          <label htmlFor="scheduleName">
            Schedule Name<label className="required">*</label>:{" "}
          </label>
          <input type="text" id="name" name="name" value={scheduleData.name} onChange={handleInputChange} required />
        </div>
        <div>
          <label htmlFor="shiftTimes">
            Shift Intervals (in minutes)<label className="required">*</label>:{" "}
          </label>
          <select
            name="shiftTimes"
            id="shiftTimes"
            value={scheduleData.shiftTimes ?? ""}
            onChange={handleInputChange}
            required
          >
            <option value="" disabled>
              -- Select shift interval --
            </option>
            <option value="15">15</option>
            <option value="20">20</option>
            <option value="30">30</option>
            <option value="60">60</option>
          </select>
        </div>
        <div>
          <label htmlFor="scheduleDuration">
            Schedule Duration<label className="required">*</label>:{" "}
          </label>
          <input
            type="time"
            id="startTime"
            name="startTime"
            value={formatTimeString(scheduleData.hours.start.hour as number, scheduleData.hours.start.minute as number)}
            step={scheduleData.shiftTimes ? Number(scheduleData.shiftTimes) * 60 : 15}
            onChange={handleStartTimeChange}
            required
          />
          <label> to </label>
          <input
            type="time"
            id="endTime"
            name="endTime"
            value={formatTimeString(scheduleData.hours.end.hour as number, scheduleData.hours.end.minute as number)}
            step={scheduleData.shiftTimes ? Number(scheduleData.shiftTimes) * 60 : 15}
            onChange={handleEndTimeChange}
            required
          />
          <br />
          <label>
            Dates<label className="required">*</label>:{" "}
          </label>
          <DatePicker
            multiple
            value={scheduleData.dates?.map((d) => new DateObject(d)) || []}
            onChange={handleDateChange}
            required
          />
          <br />
        </div>
        <div>
          <label htmlFor="peoplePerShift">
            Workers per shift<label className="required">*</label>:{" "}
          </label>
          <input
            type="number"
            id="pplPerShift"
            name="pplPerShift"
            step="1"
            min="1"
            value={scheduleData.pplPerShift ?? ""}
            onChange={handleInputChange}
            required
          />
        </div>
        <div>
          <label htmlFor="maxShiftLength">Maximum Shift Duration (in minutes): </label>
          <input
            type="number"
            id="maxShiftDuration"
            name="maxShiftDuration"
            step={scheduleData.shiftTimes as number}
            value={scheduleData.maxShiftLength ?? ""}
            onChange={handleInputChange}
          />
        </div>
        <div>
          <label htmlFor="maxShifts">Maximum Shifts per Worker: </label>
          <input
            type="number"
            id="maxShifts"
            name="maxShifts"
            step="1"
            value={scheduleData.maxShifts ?? ""}
            onChange={handleInputChange}
          />
        </div>
        <button type="submit" className="scheduleBtn" onClick={checkForNull}>
          Create Schedule
        </button>
      </form>
      <button
        className="rightButton"
        onClick={() => {
          navigate("/");
        }}
      >
        Back to Home
      </button>{" "}
    </div>
  );
};

export default NewSchedule;

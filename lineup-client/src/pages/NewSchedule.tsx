import type { DateDay, Time, TimeRange, ValidHours, ValidMinutes } from "@/types";
//import { useAuth0 } from "@auth0/auth0-react";
import { useNavigate } from "react-router";
import React from "react";

interface ScheduleData {
  name: string; //the name of the event
  shiftTimes: ValidMinutes; //how the availability intervals are determined
  dates: DateDay[]; //the dates being scheduled
  hours: TimeRange; //the hours throughout the day that need covered
  pplPerShift: number; //how many people should work simultaneously

  // optional parameters
  maxShiftLength: number | undefined; //maximum length of time a single person can work continuously, in minutes
  maxShifts: number | undefined; //maximum number of shifts a single person can work
}

const NewSchedule = () => {
  const navigate = useNavigate();

  const [scheduleData, setScheduleData] = React.useState<ScheduleData>({
    name: "",
    shiftTimes: 0 as ValidMinutes,
    dates: [{ date: "1/1", day: "Thursday" }] as DateDay[],
    hours: { start: { hour: 9, minute: 0 }, end: { hour: 17, minute: 0 } } as TimeRange,
    pplPerShift: 1,
    //optional parameters
    maxShiftLength: undefined,
    maxShifts: undefined,
  });

  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = event.target;
    setScheduleData({
      ...scheduleData,
      [name]: value,
    });
  };

  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    console.log(
      "Updated name:",
      scheduleData.name,
      "; Updated avail. interval: ",
      scheduleData.shiftTimes,
      "; Updated duration: ",
      scheduleData.hours.start,
      scheduleData.hours.end,
      "; Updated pplpershift: ",
      scheduleData.pplPerShift,
      "; Updated maximum shift time: ",
      scheduleData.maxShiftLength,
      "; Updated maximum shifts: ",
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

  return (
    <div className="newSchedule">
      <form onSubmit={handleSubmit}>
        <div>
          <label htmlFor="scheduleName">Schedule Name: </label>
          <input type="text" id="name" name="name" value={scheduleData.name} onChange={handleInputChange} required />
        </div>
        <div>
          <label htmlFor="">Shift Intervals (in minutes): </label>
          <select
            name="shiftTimes"
            id="shiftTimes"
            value={scheduleData.shiftTimes}
            onChange={handleInputChange}
            required
          >
            <option value="0" disabled>
              -- Select shift interval --
            </option>
            <option value="15">15</option>
            <option value="20">20</option>
            <option value="30">30</option>
            <option value="60">60</option>
          </select>
        </div>
        <div>
          <label htmlFor="scheduleDuration">Schedule Duration: </label>
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
          <label> on </label>
          <br />
          <label>//TODO: date picker</label>
          <br />
        </div>
        <div>
          <label htmlFor="peoplePerShift">Workers per shift: </label>
          <input
            type="number"
            id="pplPerShift"
            name="pplPerShift"
            step="1"
            min="1"
            value={scheduleData.pplPerShift}
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
            value={scheduleData.maxShiftLength}
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
            value={scheduleData.maxShifts}
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

import type { DateDay, Time, TimeRange, ValidMinutes } from "@/types";
//import { useAuth0 } from "@auth0/auth0-react";
import { queryClient, useApi } from "@/utils/api";
import { addToasts } from "@/utils/db";
import toast from "react-hot-toast";
import {
  convertToDateDays,
  formatTimeForInput,
  getValidMinutesForInterval,
  parseTimeString,
  toMinutes,
} from "@/utils/time.ts";
import { useQuery, useMutation } from "@tanstack/react-query";
import React from "react";
import DatePickerModule, { DateObject } from "react-multi-date-picker";
import { useNavigate } from "react-router";
import "./newschedule.css";
import "../dateinput.css";
import { ArrowLeftIcon } from "@radix-ui/react-icons";
import { useParams } from "react-router";
import { Calendar } from "@/components/Calendar";
// TODO: Replace with availability viewable cell when that's made
import { FillableCell } from "@/components/CalendarCells";

// This is because importing DatePicker directly didn't work with Vite for some reason
// Trust me that this is somehow the most elegant solution I could find
// @ts-expect-error: react-multi-date-picker is funky with Vite
const DatePicker = DatePickerModule.default || DatePickerModule;

interface ScheduleData {
  name: string; //the name of the event
  shiftTimes: ValidMinutes | "" | undefined; //how the availability intervals are determined
  dates: Date[] | undefined; //the dates being scheduled (js Date version)
  dateDays: DateDay[]; //the dates being schedules (DateDay version)
  hours: TimeRange; //the hours throughout the day that need covered
  pplPerShift: number | undefined; //how many people should work simultaneously

  // optional parameters
  maxShiftLength: number | undefined; //maximum length of time a single person can work continuously, in minutes
  maxShifts: number | undefined; //maximum number of shifts a single person can work
}

const ViewEditSchedule = () => {
  const navigate = useNavigate();
  const { fetchWithAuth } = useApi();
  const { guid } = useParams<{ guid: string }>();

  const {
    data: schedule,
    isLoading,
    isError,
  } = useQuery({
    queryKey: ["schedule", guid],
    queryFn: () =>
      fetchWithAuth(`/api/schedule/${guid}`).then(async (res) => {
        if (!res.ok) {
          toast.error(<b>Failed to fetch schedule</b>, { id: "fetch-schedule-error", duration: Infinity });
          throw new Error("Failed to fetch schedule");
        } else {
          toast.dismiss("fetch-schedule-error");
        }
        const resJson = await res.json();
        return resJson;
      }),
  });

  type CreateScheduleProps = {
    name: string;
    dateCoverage: string[];
    startTime: string;
    endTime: string;
    schedulePreferences: {
      minutesPerSlot: number;
      shiftIntervals: number;
      usersPerShift: number;
      maximumShiftDurationMinutes: number;
      maximumShiftsPerWorker: number;
    };
  };

  const createScheduleMutation = useMutation({
    mutationFn: async (newSchedule: CreateScheduleProps) => {
      const res = await fetchWithAuth(`/api/schedule`, {
        method: "POST",
        body: JSON.stringify(newSchedule),
        headers: {
          "Content-Type": "application/json",
        },
      });

      if (!res.ok) {
        throw new Error("Failed to add schedule");
      }

      return res.json();
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["schedules"] });
      navigate("/");
    },
  });

  const [scheduleData, setScheduleData] = React.useState<ScheduleData>({
    name: "",
    shiftTimes: "",
    dates: undefined,
    dateDays: [{ date: "1/1", day: "Thursday" }] as DateDay[],
    hours: { start: { hour: 9, minute: 0 }, end: { hour: 17, minute: 0 } } as TimeRange,
    pplPerShift: undefined,
    //optional parameters
    maxShiftLength: undefined,
    maxShifts: undefined,
  });

  React.useEffect(() => {
    if (!schedule) return;

    const jsDates = schedule.dateCoverage?.map((d: string) => new Date(d)) ?? [];
    console.log("Fetched schedule:", schedule);
    setScheduleData({
      name: schedule.name,
      shiftTimes: schedule.schedulePreferences?.minutesPerSlot || 15,
      dates: jsDates,
      dateDays: convertToDateDays(jsDates),
      hours: {
        start: parseTimeString(schedule.startTime)!,
        end: parseTimeString(schedule.endTime)!,
      },
      pplPerShift: schedule.schedulePreferences?.usersPerShift || 1,
      maxShiftLength: schedule.schedulePreferences?.maximumShiftDurationMinutes ?? undefined,
      maxShifts: schedule.schedulePreferences?.maximumShiftsPerWorker ?? undefined,
    });
  }, [schedule]);

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
      "Name: ",
      scheduleData.name,
      "; Shift intervals: ",
      scheduleData.shiftTimes,
      "; Dates: ",
      scheduleData.dates,
      "; DateDays: ",
      scheduleData.dateDays,
      "; Hours: ",
      scheduleData.hours,
      "; PplPerShift: ",
      scheduleData.pplPerShift,
      "; maxShiftLength: ",
      scheduleData.maxShiftLength,
      "; maxShifts: ",
      scheduleData.maxShifts,
    );
    switch (checkValidTimeRange(scheduleData.shiftTimes, scheduleData.hours)) {
      case 1:
        alert("Cannot have Schedule Duration end before it starts.");
        return;
      case 2:
        alert("Duration start time minutes must be compatible with chosen shift interval.");
        return;
      case 3:
        alert("Duration end time minutes must be compatible with chosen shift interval.");
        return;
      case 4:
        alert("Total Duration must be divisible by chosen shift interval.");
        return;
      case 0:
        break;
    }

    addToasts(
      createScheduleMutation.mutateAsync({
        name: scheduleData.name,
        dateCoverage: scheduleData.dates?.map((d) => d.toISOString().split("T")[0]) || [],
        startTime: formatTimeForInput(scheduleData.hours.start),
        endTime: formatTimeForInput(scheduleData.hours.end),
        schedulePreferences: {
          minutesPerSlot: Number(scheduleData.shiftTimes),
          shiftIntervals: Number(scheduleData.shiftTimes),
          usersPerShift: Number(scheduleData.pplPerShift),
          maximumShiftDurationMinutes: scheduleData.maxShiftLength ?? 1440,
          maximumShiftsPerWorker: scheduleData.maxShifts ?? 99999,
        },
      }),
    );
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

  const checkValidTimeRange = (intervalLength: ValidMinutes | "" | undefined, hours: TimeRange): number => {
    if (!intervalLength) return 0;

    const interval = Number(intervalLength);
    const validMinutes = getValidMinutesForInterval(interval);
    const startMinutes = toMinutes(hours.start);
    const endMinutes = toMinutes(hours.end, true);

    if (startMinutes >= endMinutes) return 1;
    if (!validMinutes.includes(hours.start.minute)) return 2;
    if (!validMinutes.includes(hours.end.minute)) return 3;
    if ((endMinutes - startMinutes) % interval !== 0) return 4;

    return 0;
  };

  if (isLoading || !scheduleData) return <div>Loading...</div>;
  if (isError) return <div>Error loading schedule.</div>;

  return (
    <div className="newSchedule">
      <button
        className="returnButton"
        onClick={() => {
          navigate("/");
        }}
      >
        <ArrowLeftIcon className="backIcon" />
        Home
      </button>{" "}
      <div>
        <h3 className="pageHeader">{scheduleData.name}</h3>
        <h4></h4>
      </div>
      <hr />
      <Calendar
        Cell={FillableCell}
        minutesPerCell={scheduleData.shiftTimes as ValidMinutes}
        dates={scheduleData.dateDays}
        range={scheduleData.hours}
      ></Calendar>
      <form onSubmit={handleSubmit}>
        <div>
          <label>Dates: </label>
          <br />
          <DatePicker
            inputClass="input"
            className="purple"
            multiple
            value={scheduleData.dates?.map((d) => new DateObject(d)) || []}
            onChange={handleDateChange}
            minDate={new Date()}
            required
          />
          <br />
        </div>
        <div>
          <label htmlFor="peoplePerShift">Workers per shift: </label>
          <br />
          <input
            className="input"
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
          <label htmlFor="maxShiftLength">Maximum Shift Duration (in minutes):</label>
          <br />
          <input
            className="input"
            type="number"
            id="maxShiftLength"
            name="maxShiftLength"
            step={scheduleData.shiftTimes ? Number(scheduleData.shiftTimes) : 60}
            min={scheduleData.shiftTimes ? Number(scheduleData.shiftTimes) : 60}
            value={scheduleData.maxShiftLength ?? ""}
            onChange={handleInputChange}
          />
        </div>
        <div>
          <label htmlFor="maxShifts">Maximum Shifts per Worker:</label>
          <br />
          <input
            className="input"
            type="number"
            id="maxShifts"
            name="maxShifts"
            step="1"
            min="1"
            value={scheduleData.maxShifts ?? ""}
            onChange={handleInputChange}
          />
        </div>
        <br />
        <div className="submitContainer">
          <button type="submit" className="submitBtn">
            Confirm Changes
          </button>
        </div>
      </form>
    </div>
  );
};

export default ViewEditSchedule;

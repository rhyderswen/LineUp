import { Calendar } from "@/components/Calendar";
import { FillableCell } from "@/components/CalendarCells";
import { convertToDateDays, parseTimeString } from "@/utils/time";
import { useLoaderData } from "react-router";

const Availability = () => {
  const data = useLoaderData();
  console.log(data);

  const handleSubmit = (event: React.SubmitEvent<HTMLFormElement>) => {
    event.preventDefault();
    const formData = new FormData(event.currentTarget);
    console.log(formData);
    if (formData.get("calendarSelected") === "") {
      alert("Please select at least one time slot.");
      return;
    }

    // TODO actually submit it when the API is ready
  };

  return (
    <div className="availabilityRoot">
      <div className="scheduleName">
        Add availability for <b>{data.name}</b>
      </div>
      <form onSubmit={handleSubmit} className="newSchedule">
        <div className="inputGroup">
          <div>
            <label htmlFor="name" className="required">
              Full Name
            </label>
            <br />
            <input className="input" type="text" id="name" name="name" required />
          </div>
          <div>
            <label htmlFor="email" className="required">
              Email
            </label>
            <br />
            <input className="input" type="email" id="email" name="email" required />
          </div>
        </div>
        <div>
          <label className="availabilityLabel required">Availability</label>
          <Calendar
            Cell={FillableCell}
            minutesPerCell={data.schedulePreferences?.minutesPerSlot || 15}
            dates={convertToDateDays(data.dateCoverage?.map((d: string) => new Date(d)) ?? [])}
            range={{
              start: parseTimeString(data.startTime)!,
              end: parseTimeString(data.endTime)!,
            }}
          ></Calendar>
        </div>
        <div className="submitContainer">
          <button type="submit" className="submitBtn">
            Submit
          </button>
        </div>
      </form>
    </div>
  );
};

export default Availability;

import { Calendar } from "@/components/Calendar";
import { ColoredCell, FillableCell } from "@/components/CalendarCells";
import { queryClient, useApi } from "@/utils/api";
import { addToasts, loaderQuery } from "@/utils/db";
import { parseTimeString } from "@/utils/time";
import { useMutation, useQuery } from "@tanstack/react-query";
import { useNavigate, useParams } from "react-router";

const Availability = () => {
  const navigate = useNavigate();
  const { fetchWithAuth } = useApi();
  const { guid } = useParams();
  const { data } = useQuery(loaderQuery("/api/schedule/{}", guid!));
  const scheduleGenerated = data.shiftAssignments.length > 0;

  console.log(data);

  type CreateAvailabilityProps = {
    userName: string;
    userEmail: string;
    availabilitySlots: string[]; // full of ISO strings
  };

  const createAvailabilityMutation = useMutation({
    mutationFn: async (newAvailability: CreateAvailabilityProps) => {
      console.log(newAvailability);
      const res = await fetchWithAuth(`/api/schedule/${guid}/createAvailability`, {
        method: "POST",
        body: JSON.stringify(newAvailability),
        headers: {
          "Content-Type": "application/json",
        },
      });

      if (!res.ok) {
        throw new Error("Failed to create availability");
      }

      return res;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["availability"] });
      navigate("/");
    },
  });

  const handleSubmit = (event: React.SubmitEvent<HTMLFormElement>) => {
    event.preventDefault();
    const formData = new FormData(event.currentTarget);

    if ((formData.get("name") as string).trim() === "" || (formData.get("email") as string).trim() === "") {
      alert("You must enter a name with letters!");
      return;
    }

    if (formData.get("calendarSelected") === "") {
      alert("Please select at least one time slot.");
      return;
    }

    addToasts(
      createAvailabilityMutation.mutateAsync({
        userName: (formData.get("name") as string).trim(),
        userEmail: (formData.get("email") as string).trim(),
        availabilitySlots: (formData.get("calendarSelected") as string).split(","),
      }),
    );
  };

  if (!data) return <div>Loading...</div>;

  return (
    <div className="availabilityRoot">
      {scheduleGenerated ? (
        <>
          <div className="scheduleName">
            Schedule for <b>{data.name}</b>
          </div>
          <br />
          <Calendar
            Cell={ColoredCell}
            minutesPerCell={data.schedulePreferences?.minutesPerSlot || 15}
            dates={
              data.dateCoverage?.map((d: string) => {
                const [year, month, day] = d.split("-").map(Number);
                return new Date(year, month - 1, day);
              }) ?? []
            }
            range={{
              start: parseTimeString(data.startTime)!,
              end: parseTimeString(data.endTime)!,
            }}
          />
        </>
      ) : (
        <>
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
                dates={
                  data.dateCoverage?.map((d: string) => {
                    const [year, month, day] = d.split("-").map(Number);
                    return new Date(year, month - 1, day);
                  }) ?? []
                }
                range={{
                  start: parseTimeString(data.startTime)!,
                  end: parseTimeString(data.endTime)!,
                }}
              />
            </div>
            <div className="submitContainer">
              <button type="submit" className="submitBtn" disabled={createAvailabilityMutation.isPending}>
                Submit
              </button>
            </div>
          </form>
        </>
      )}
    </div>
  );
};

export default Availability;

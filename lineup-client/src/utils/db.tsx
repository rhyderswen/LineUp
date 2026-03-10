import { toast } from "react-hot-toast";

// eslint-disable-next-line @typescript-eslint/no-explicit-any
function addToasts(promise: Promise<any>) {
  toast.promise(promise, {
    loading: "Submitting...",
    success: <b>Success!</b>,
    error: (err) => <b>Error: {err.message}</b>,
  });
}

export { addToasts };

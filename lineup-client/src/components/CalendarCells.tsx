import type { DateDay, Time } from "@/types";
import { formatDateTime, formatTime } from "@/utils/time";
import React from "react";

export interface CalendarCellProps {
  time: Time;
  date: DateDay;
  selectedCells: string[];
  setSelectedCells: React.Dispatch<React.SetStateAction<string[]>>;
  isPointerDown: boolean;
  setIsPointerDown: React.Dispatch<React.SetStateAction<boolean>>;
  isEnablingCells: boolean;
  setIsEnablingCells: React.Dispatch<React.SetStateAction<boolean>>;
  setCurrentlyFocusedCell?: React.Dispatch<React.SetStateAction<string | null>>;
  colors: { [key: string]: string };
}

const FillableCell = ({
  time,
  date,
  selectedCells,
  setSelectedCells,
  isPointerDown,
  setIsPointerDown,
  isEnablingCells,
  setIsEnablingCells,
}: CalendarCellProps) => {
  const isClicked = selectedCells.includes(formatDateTime(date.date, time));

  function updateCell() {
    if (isClicked) {
      setSelectedCells((cells) => cells.filter((cell) => cell !== formatDateTime(date.date, time)));
    } else {
      setSelectedCells((cells) => [...cells, formatDateTime(date.date, time)]);
    }
  }

  function onPointerDown() {
    setIsPointerDown(true);
    setIsEnablingCells(!isClicked);
    updateCell();
  }

  function onPointerEnter() {
    if (isPointerDown && isClicked !== isEnablingCells) {
      updateCell();
    }
  }

  return (
    <button
      type="button"
      onPointerDown={onPointerDown}
      onPointerUp={() => setIsPointerDown(false)}
      onPointerEnter={onPointerEnter}
      className={"unstyledButton calendarInnerCell" + (isClicked ? " clicked" : "")}
      title={date.day + " " + formatTime(time)}
    ></button>
  );
};

const ColoredCell = ({ time, date, setCurrentlyFocusedCell, colors }: CalendarCellProps) => {
  return (
    <div
      onPointerEnter={() => setCurrentlyFocusedCell && setCurrentlyFocusedCell(formatDateTime(date.date, time))}
      onPointerLeave={() => setCurrentlyFocusedCell && setCurrentlyFocusedCell(null)}
      className={"calendarInnerCell"}
      title={date.day + " " + formatTime(time)}
      style={{ backgroundColor: colors[formatDateTime(date.date, time)] ?? "transparent" }}
    ></div>
  );
};

export { ColoredCell, FillableCell };

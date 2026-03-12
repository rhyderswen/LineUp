/**
 * @vitest-environment jsdom
 */
import { describe, it, expect, vi, beforeEach, afterEach } from "vitest";
import { render, screen } from "@testing-library/react";
import { ColoredCell, FillableCell } from "../components/CalendarCells";
import "@testing-library/jest-dom/vitest";

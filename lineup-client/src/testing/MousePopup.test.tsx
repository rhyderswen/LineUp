/**
 * @vitest-environment jsdom
 */
import { describe, it, expect, vi, beforeEach, afterEach } from "vitest";
import { render, screen } from "@testing-library/react";
import { MousePopup } from "../components/MousePopup";
import "@testing-library/jest-dom/vitest";

import React from "react";
import { describe, it, expect, vi, beforeEach } from "vitest";
import { render, screen, fireEvent } from "@testing-library/react";
import "@testing-library/jest-dom/vitest";
import Topbar from "../components/Topbar";
import { useAuth0 } from "@auth0/auth0-react";
import { useNavigation } from "react-router";

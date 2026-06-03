import axios from "axios";

// Main API — WomensSection backend
export default axios.create({
  baseURL: "http://localhost:5182/api",
  withCredentials: true,
});

// Auth API — separate Auth project on port 5000
export const authApi = axios.create({
  baseURL: "http://localhost:5000/api",
  withCredentials: true,
});

// Order Service — centralized cart + orders (port 5200)
export const orderApi = axios.create({
  baseURL: "http://localhost:5200/api",
  withCredentials: true,
});
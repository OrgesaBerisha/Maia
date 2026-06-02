import axios from "axios";

// Main API — WomensSection backend
export default axios.create({
  baseURL: "http://localhost:5182/api",
  withCredentials: true,   // sends the jwt cookie automatically
});

// Auth API — separate Auth project on port 5000
export const authApi = axios.create({
  baseURL: "http://localhost:5000/api",
  withCredentials: true,   // needed for cookie-based auth
});
export interface LoginResponse {
  userId: number;
  firstName: string;
  profilePicture: string;
  role: number | boolean;
}
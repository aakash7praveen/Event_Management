import { Event } from "./event.model";

export interface User {
    user_Id: number;
    first_Name: string;
    middle_Name?: string;
    last_Name: string;
    email: string;
    phone_Number?: string;
    profile_Picture?: string;
    role: boolean;
    created_Ts: string;
    enc_Password: string;

    name?: string;       
    phone?: string;      
    expanded?: boolean;  
    events?: Event[]; 
}
export interface Event {
  id: number;                
  title: string;
  description: string;
  startDt: string;           
  endDt: string;
  location: string;
  category: string;
  duration: string;
  maxAttendees?: number;
  createdBy?: number;
  createdTs?: string;
  updatedTs?: string;
  deleteInd: boolean;
}

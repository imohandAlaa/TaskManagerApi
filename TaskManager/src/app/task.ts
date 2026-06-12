export interface Task {
  Id: number;
  Title: string;
  Description?: string;
  IsCompleted: boolean;
  CreatedAt: Date;
}
export const dummyTask: Task[] = [
  {
    Id: 1,
    Title: 'first dummy task',
    Description: 'finish my task',
    IsCompleted: true,
    CreatedAt: new Date(),
  },
  {
    Id: 1,
    Title: 'Second dummy task',
    Description: 'finish my task',
    IsCompleted: true,
    CreatedAt: new Date(),
  },
];

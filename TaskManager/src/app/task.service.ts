import { Injectable, inject, input } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Task } from './task';

// angular httpclient injectable dependency
@Injectable({ providedIn: 'root' })
export class TaskService {
  private http = inject(HttpClient); // Inject the client
  private url: string = 'http://localhost:5266/api/tasks'; // our asp.net server endpoint
  // first endpoint function for fetching all tasks with Observalbe reactive callback function
  getTasks(): Observable<any[]> {
    return this.http.get<any[]>(this.url);
  }
  // create task function
  createNewTask(inputTask: Task): Observable<Task> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });
    return this.http.post<Task>(this.url, inputTask, { headers });
  }
  // delete task function
  deleteTask(id: any): Observable<void> {
    return this.http.delete<void>(`${this.url}/${id}`);
  }
  // update task
  updateTask(id: any, newTask: Task): Observable<Task> {
    return this.http.put<Task>(`${this.url}/${id}`, newTask);
  }
}

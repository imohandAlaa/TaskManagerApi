import { Component, signal, OnInit, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Task, dummyTask } from './task';
import { NavbarComponent } from './Navbar/navbar.component';
import { CardFooterExample } from './Card/card.component';
import { TaskService } from './task.service';
@Component({
  selector: 'app-root',
  imports: [RouterOutlet, NavbarComponent, CardFooterExample],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App implements OnInit {
  private taskService = inject(TaskService);
  tasks: any = signal<Task[]>([]);

  ngOnInit(): void {
    this.taskService.getTasks().subscribe({
      next: (data) => this.tasks.set(data),
      error: (err) => console.error('Failed to Tasks users', err),
    });
  }
}

// start with getting all tasks in console
// create new Task post request with toaster

import { Component, signal, OnInit, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Task } from './task';
import { NavbarComponent } from './Navbar/navbar.component';
import { CardFooterExample } from './Card/card.component';
import { TaskService } from './task.service';
@Component({
  selector: 'app-root',
  imports: [RouterOutlet, NavbarComponent, CardFooterExample],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
// major app component
export class App implements OnInit {
  // inject http client dependency
  private taskService = inject(TaskService);
  // intilizing array of tasks
  tasks: any = signal<Task[]>([]);
  // on app start running this function to fetch first set of tasks
  ngOnInit(): void {
    this.taskService.getTasks().subscribe({
      next: (data) => {
        this.tasks.set(data);
        console.log(data);
      },
      error: (err) => console.error('Failed to Tasks users', err),
    });
  }
}


import { Component, input, inject } from '@angular/core';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatButtonModule } from '@angular/material/button';
import { TaskService } from '../task.service';

@Component({
  selector: 'card-footer-example',
  templateUrl: './card.component.html',
  styleUrl: './card.component.css',
  imports: [MatCardModule, MatChipsModule, MatProgressBarModule, MatButtonModule],
})
// task card
export class CardFooterExample {
  private taskService = inject(TaskService); //inject http client

  // input values pass by app.ts component
  taskId = input(0);
  title = input('');
  description = input('');
  IsCompleted = input(true);
  CreatedAt = input(new Date());
  // delete task main function linked with http client
  deleteTaskButton() {
    this.taskService.deleteTask(this.taskId()).subscribe({
      next: (res) => {
        console.log('task Deleted');
        setTimeout(() => {
          location.reload();
        }, 500);
      },
      error: (err) => console.log(err),
    });
  }
  // update task status function linked with http client
  updateTaskCheckBox() {
    this.taskService
      .updateTask(this.taskId(), {
        Id: this.taskId(),
        IsCompleted: this.IsCompleted(),
        Title: this.title(),
        Description: this.description(),
        CreatedAt: this.CreatedAt(),
      })
      .subscribe({
        next: (res) => console.log('task updated'),
        error: (err) => console.log(err),
      });
  }
}

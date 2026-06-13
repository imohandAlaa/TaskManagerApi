import { Component, inject } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { TaskService } from '../task.service';
@Component({
  selector: 'app-form-modal',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './form.component.html',
})
// main task form component
export class FormModalComponent {
  taskForm: FormGroup;
  private taskService = inject(TaskService);
  constructor(
    private dialogRef: MatDialogRef<FormModalComponent>, // dialogRef for controlling the model
    private fb: FormBuilder,
  ) {
    // setting form validators with form builder
    this.taskForm = this.fb.group({
      Title: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
      Description: [''],
      IsCompleted: [false],
      CreatedAt: [new Date()],
    });
  }
  // on submit function with sending the post request if the form values is valid
  onSubmit() {
    if (this.taskForm.valid) {
      this.taskService.createNewTask(this.taskForm.value).subscribe({
        next: (res) => {
          console.log('task submited');
          setTimeout(() => {
            location.reload();
          }, 500);
        },
        error: (err) => console.log(err),
      });
      this.dialogRef.close();
    }
  }

  cancel() {
    this.dialogRef.close();
  }
}

import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-form-modal',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './form.component.html',
})
export class FormModalComponent {
  taskForm: FormGroup;

  constructor(
    private dialogRef: MatDialogRef<FormModalComponent>,
    private fb: FormBuilder,
  ) {
    this.taskForm = this.fb.group({
      Title: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
      Description: [''],
      IsCompleted: [false],
      CreatedAt: [new Date()],
    });
  }

  onSubmit() {
    if (this.taskForm.valid) {
      console.log('Form Submitted!', this.taskForm.value);
      this.dialogRef.close();
    }
  }

  cancel() {
    this.dialogRef.close();
  }
}

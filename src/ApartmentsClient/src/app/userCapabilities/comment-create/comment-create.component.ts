import { Component, OnInit } from '@angular/core';
import { NgForm, FormControl, FormGroup, FormBuilder } from '@angular/forms';

import { ApartmentView } from 'src/app/core/nswag.generated.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';

import { CommentUserService }  from 'src/app/core/nswag.generated.service';

import { ApartmentDTO, AddApartment } from 'src/app/core/nswag.generated.service';
import { AddressDTO, AddAddress } from 'src/app/core/nswag.generated.service';
import { AddComment, CommentDTO } from 'src/app/core/nswag.generated.service';

@Component({
  selector: 'app-comment-create',
  templateUrl: './comment-create.component.html',
  styleUrls: ['./comment-create.component.scss']
})
export class CommentCreateComponent implements OnInit {

  commentForm: FormGroup;

  apartmentId: string;
  title: string;
  text: string;

  constructor(
    private formBuilder: FormBuilder,
    private commentService: CommentUserService,
    private route: ActivatedRoute
  ) { 
    this.commentForm = this.formBuilder.group({
      apartmentId: this.getApartmentId(),
      title: '',
      text: ''
    })
  }

  ngOnInit(): void {
  }

  getApartmentId(): string {
   const id = this.route.snapshot.paramMap.get('id');
   return id;
  }

  add(){
    this.commentService.createComment(this.commentForm.value)
    .subscribe(data =>
      this.commentForm.reset()
    );
  }
}

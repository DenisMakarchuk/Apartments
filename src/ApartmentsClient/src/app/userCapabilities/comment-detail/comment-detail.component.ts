import { Component, OnInit } from '@angular/core';
import { NgForm, FormControl, FormGroup, FormBuilder } from '@angular/forms';

import { ApartmentView } from 'src/app/core/nswag.generated.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';

import { CommentUserService, UserService }  from 'src/app/core/nswag.generated.service';

import { ApartmentDTO, AddApartment } from 'src/app/core/nswag.generated.service';
import { AddressDTO, AddAddress } from 'src/app/core/nswag.generated.service';
import { AddComment, CommentDTO } from 'src/app/core/nswag.generated.service';

import * as jwt_decode from 'jwt-decode';

@Component({
  selector: 'app-comment-detail',
  templateUrl: './comment-detail.component.html',
  styleUrls: ['./comment-detail.component.scss']
})
export class CommentDetailComponent implements OnInit {

  comment: CommentDTO;
  isUpdating = false;

  constructor(
    public router: Router,
    private route: ActivatedRoute,
    private commentService: CommentUserService,
    private location: Location,
    private authService: UserService
  ) { }

  get isAuther(){
    var token = this.authService.getToken();
      
    var decodedoken = jwt_decode(token);
    var currentUserId = decodedoken['id'];

    if (currentUserId === this.comment.authorId) {
      return true;
    }
    return false;
  }

  ngOnInit(): void {
    this.getComment();
  }

  getComment(): void {
    const id = this.route.snapshot.paramMap.get('id');
    this.commentService.getCommentById(id)
      .subscribe(comment => this.comment = comment);
  }

  updating(){
    this.isUpdating = true;
  }

  save(): void {
    this.commentService.updateComment(this.comment)
      .subscribe(comment => {
        this.comment = comment;
        
        this.isUpdating = false;
      });
  }

  delete(){
    const id = this.route.snapshot.paramMap.get('id');
    this.commentService.deleteCommentById(id)    
    .subscribe(()=>this.goBack());

  }

  goBack(): void {
    this.location.back();
  }

}

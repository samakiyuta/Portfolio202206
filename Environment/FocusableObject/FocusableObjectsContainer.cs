using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;
using System;
public class FocusableObjectsContainer : MonoBehaviour
{
    public List<FocusableObject> valueList => _reactiveCollection.ToList();
    public IObservable<CollectionAddEvent<FocusableObject>> addObservable => _reactiveCollection.ObserveAdd();
    public IObservable<CollectionRemoveEvent<FocusableObject>> removeObservable => _reactiveCollection.ObserveRemove();
    private readonly ReactiveCollection<FocusableObject> _reactiveCollection = new ReactiveCollection<FocusableObject>(new List<FocusableObject>());
    public void Add(FocusableObject obj)
    {
        //既に追加する要素が含まれていたら追加しない
        if (!_reactiveCollection.FirstOrDefault(it => it == obj))
        {
            _reactiveCollection.Add(obj);
        }
    }
    public void Remove(FocusableObject obj)
    {
        _reactiveCollection.Remove(obj);
    }
    public FocusableObject GetLasted()
    {
        return _reactiveCollection.Last();
    }
}

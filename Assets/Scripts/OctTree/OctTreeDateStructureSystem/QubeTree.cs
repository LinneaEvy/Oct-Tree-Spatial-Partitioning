using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class QubeTree
{
    private float boundary;
    private int capacity;//make this dictionary to so one can look up the previous bird pos 
    private Dictionary<int, float3> Flyers;
    private static Dictionary<int, float3> AllBirds = new Dictionary<int, float3>();
    private bool divited;
    private Bounds thisTree;
    private QubeTree[] subdivision;
    private float3 pos;

    public QubeTree(float boundary, int maxInside, float3 position)
    {
        this.boundary = boundary;
        this.capacity = maxInside;
        Flyers = new Dictionary<int, float3>();
        
        this.divited = false;
        thisTree = new Bounds(position, new float3(boundary, boundary, boundary));
        pos = position;
        
        subdivision = new QubeTree[8];
    }

    public void ClearTree()
    {
        subdivision = new QubeTree[8];
        Flyers = new Dictionary<int, float3>();
        AllBirds = new Dictionary<int, float3>();
        divited = false;
    }
    public List<Bounds> getBounds(List<Bounds> res, int recLevel)
    {
        res.Add(thisTree);
        recLevel++;
        if (divited)
        {
            subdivision[0].getBounds(res,recLevel);
            subdivision[1].getBounds(res,recLevel);
            subdivision[2].getBounds(res,recLevel);
            subdivision[3].getBounds(res,recLevel);
            subdivision[4].getBounds(res,recLevel);
            subdivision[5].getBounds(res,recLevel);
            subdivision[6].getBounds(res,recLevel);
            subdivision[7].getBounds(res,recLevel);
        }

        return res;
    }

    public void updateBird(float3 posi, int index)
    {
        //compare prev pos with new and detect chunkchange 
        // you move recursively through the tree upon arrival of the correct chung check if subdivision is necessary by checking the birds array in that chunk
        
        RemoveBird(index, null);
        insert(posi, index, false);
        
    }

    private bool changed(float3 pos, float3 newpos, int precisionSubdivIndex)//would be stupid to traverse the whole tree 
    {
        float cellwith = boundary / precisionSubdivIndex;
        for (int i = 0; i < 3; i++)
        {
            if (pos[i] / cellwith != newpos[i] / cellwith)
            {
                return true;
            }
        }
        
        return false;
    }
    private void RemoveBird(int index, QubeTree prevTree)
    {
        if (contains(AllBirds[index]))//will be true even if arr is empty because it checks bounds
        {
            if (!divited)//wir sind in dem leaf wo der bird drin ist
            {
                Flyers.Remove(index);
                if (prevTree == null)
                {
                    return;
                }
                Dictionary<int, float3> BirdAmount = new Dictionary<int, float3>();//we collect all birds from the prev bigger chunk
                for (int i = 0; i < prevTree.subdivision.Length; i++)
                {
                    foreach (var var in prevTree.subdivision[i].Flyers)
                    {
                        BirdAmount.Add(var.Key, var.Value);
                    }
                }
                
                if (capacity >= BirdAmount.Count)//we look if there are less enough to unsubdivide
                {
                    prevTree.Flyers = BirdAmount;
                    prevTree.subdivision = new QubeTree[8];
                    prevTree.divited = false;
                }
            }
            if (divited)
            {
                for (int i = 0; i < subdivision.Length; i++) //we are going into each subdivision and trying to remove 
                {
                    if (subdivision[i] == null)
                    {
                    }else
                    {//the if should only be true for one of the 8
                        if (subdivision[i].contains(AllBirds[index], subdivision[i]))//look if the subdivited tree has the position inside
                        {
                            subdivision[i].RemoveBird(index, this);
                        }
                        
                    }
                    
                }
            }
        }
    }

    private void subdivide()
    {
        QubeTree backupright = new QubeTree(boundary / 2, capacity, new float3(boundary / 4, boundary / 4, boundary / 4)+pos);
        subdivision[0] = backupright;
        QubeTree backupleft = new QubeTree(boundary / 2, capacity, new float3(-(boundary / 4), boundary / 4, boundary / 4)+pos);
        subdivision[1] = backupleft;
        QubeTree backdownright = new QubeTree(boundary / 2, capacity, new float3(boundary / 4, boundary / 4, -(boundary / 4))+pos);
        subdivision[2] = backdownright;
        QubeTree backdownleft = new QubeTree(boundary / 2, capacity, new float3(-(boundary / 4), boundary / 4, -(boundary / 4))+pos);
        subdivision[3] = backdownleft;
        QubeTree frontupright = new QubeTree(boundary / 2, capacity, new float3(-(boundary / 4), -(boundary / 4), boundary / 4)+pos);
        subdivision[4] = frontupright;
        QubeTree frontupleft = new QubeTree(boundary / 2, capacity, new float3(boundary / 4, -(boundary / 4), boundary / 4)+pos);
        subdivision[5] = frontupleft;
        QubeTree frontdownright = new QubeTree(boundary / 2, capacity, new float3(-(boundary / 4), -(boundary / 4), -(boundary / 4))+pos);
        subdivision[6] = frontdownright;
        QubeTree frontdownleft = new QubeTree(boundary / 2, capacity, new float3(boundary / 4, -(boundary / 4), -(boundary / 4))+pos);
        subdivision[7] = frontdownleft;
        
        divited = true;
    }

    private bool contains(float3 position)
    {
        return thisTree.Contains(position);
        /*return (position.x >= thisTree.x - this.w &&
                position.x < this.x + this.w &&
                position.y >= this.y - this.h &&
                position.y < this.y + this.h);*/
    }
    private bool contains(float3 position, QubeTree thisTreee)
    {
        return thisTreee.thisTree.Contains(position);
        /*return (position.x >= thisTree.x - this.w &&
                position.x < this.x + this.w &&
                position.y >= this.y - this.h &&
                position.y < this.y + this.h);*/
    }
    
    private bool contains(float3 bird, float3 player, float range)
    {
        return (Vector3.Distance(bird, player)) < range;//dot product trick
    }

    public bool insert(float3 position, int index, bool firstinsert)
    {

        if (!contains(position))
        {
            //if bird not here run
            return false;
        }

        if (Flyers.Count < capacity&&!divited)
        {
            //wenn hier noch platz ist super willkommen
            Flyers.Add(index,position);
            if (firstinsert)
            {
                AllBirds.Add(index,position);
            }
            else
            {
                AllBirds[index] = position;
            }
            
            return true;
        }
        else
        {
            if (!divited)
            {
                //wenn noch nicht divited super divide it das insert versuchen wir dannach nochmal
                subdivide(); //wir sind noch nicht raus aus der funktion(da war noch kein return) danach gehen wir in die neu erstellten rein
                //move all birds into their subdicisions
                foreach (var b in Flyers)
                {
                    insert(b.Value, b.Key, false);//it is already in all diction
                }

                Flyers = new Dictionary<int, float3>();
            }

            for (int i = 0; i < subdivision.Length; i++) //we are going into each subdivision and trying to insert 
            {
                if (subdivision[i].insert(position, index, firstinsert))
                {
                    return true;
                }
            }

            return false;
        }
    }
    private bool intersects(float3 range, float3 playerPosition)//qube qube intersection
    {
        return thisTree.Intersects(new Bounds(playerPosition, range));
        /*return !(range.x - range.w > this.x + this.w ||
                 range.x + range.w < this.x - this.w ||
                 range.y - range.h > this.y + this.h ||
                 range.y + range.h < this.y - this.h);*/
    }

    public List<float3> query(float range, List<float3> found, float3 playerPosition)
    {
        //get all objects inside a range
        
        if (found == null)
        {
            //if there is not yet a list existing
            found =  new List<float3>();
            
        }
        if (!contains(playerPosition))//the player is not inside the entire tree
        {
            return found;//so we just return what we already have found
        }
        if (divited)//wenn es divited ist dann gucken wir jeden davon an
        {
            foreach (var sub in subdivision)
            {
                if (sub.contains(playerPosition))
                {
                    return sub.query(range, found, playerPosition);
                }
            }
        }
        else
        {
            found = new List<float3>() {this.pos, new float3(boundary, boundary, boundary)};
            return found;

            /* (var bird in Flyers)//we are at the lowest subdivision of a chunk and the chunk is close so we check what is in a sphere around player no longer  a cube
            {
                Debug.Log("ChunkFound");
                if (contains(bird.Value, playerPosition, range))
                {
                    found.Add(bird.Value);
                }
            }*/
        }
        return found;
    }
}    
    

    
